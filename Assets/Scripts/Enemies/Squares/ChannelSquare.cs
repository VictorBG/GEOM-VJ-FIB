using System;
using System.Collections;
using System.Data;
using System.Linq;
using Base;
using UnityEngine;

namespace Enemies.Squares
{
    public class ChannelSquare : MonoBehaviour
    {
        private enum ChannelDirection
        {
            TOP = 0,
            RIGHT = 1,
            BOTTOM = 2,
            LEFT = 3
        }

        [SerializeField] private ChannelDirection[] outChannels;
        [SerializeField] private ChannelDirection[] innerChannels;
        [SerializeField] private ChannelDirection inChannel;

        // FieldLines must be in the same order as outChannels
        [SerializeField] private SliderDoor[] linesConnections;
        [SerializeField] private MeshRenderer[] fieldLines;

        [SerializeField] private Material fieldLineMaterial;
        [SerializeField] private Material fieldLineMaterialNormal;

        [SerializeField] private float startAngle;
        [SerializeField] private float rotateAngle;
        [SerializeField] private float rotationDuration;

        // Blocks the rotation once the in channel is connected to at least an out channel
        [SerializeField] private bool blockRotation;

        private float _rotation;
        private Coroutine _rotateCoroutine;
        private AudioSource _audioSource;


        private void Start()
        {
            if (outChannels.Any(channel => channel == inChannel))
            {
                throw new InvalidConstraintException("inChannel cannot be an outChannel");
            }

            if (innerChannels.Length < 2)
            {
                throw new InvalidConstraintException("Inner channel must have 2 or more directions");
            }

            if (linesConnections.Length != outChannels.Length)
            {
                throw new Exception();
            }

            _rotation = startAngle;
            transform.Rotate(0, startAngle, 0);
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag($"Player") && _rotateCoroutine == null)
            {
                _rotateCoroutine = StartCoroutine(Rotate(1));
            }
        }

        private void CheckChannels()
        {
            if (_rotation % 90 != 0) return;

            var isEnterCovered = innerChannels.Any(channel => Convert(channel) == inChannel);
            var isExitCovered = false;

            var i = 0;
            foreach (var outChannel in outChannels)
            {
                var covered = innerChannels.Any(channel => Convert(channel) == outChannel);
                if (covered && isEnterCovered)
                {
                    Debug.Log("Out channel " + outChannel + " is covered and enter is also covered, opening door");
                    fieldLines[i].material = fieldLineMaterial;
                    linesConnections[i].OnOpen();
                    isExitCovered = true;
                }
                else
                {
                    fieldLines[i].material = fieldLineMaterialNormal;
                }

                i++;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (isExitCovered && isEnterCovered && blockRotation)
            {
                StartCoroutine(BlockObject());
            }
        }

        private ChannelDirection Convert(ChannelDirection channel)
        {
            var rot = (int) _rotation / 90;
            var ch = channel + rot;
            if (ch > ChannelDirection.LEFT)
            {
                ch -= 4;
            }

            return ch;
        }

        private IEnumerator Rotate(float dir)
        {
            var i = 0;
            foreach (var field in fieldLines)
            {
                field.material = fieldLineMaterialNormal;
                linesConnections[i].OnClose();
                i++;
            }

            _audioSource.Play();
            var diff = rotateAngle;
            while (diff > 0)
            {
                var degrees = Time.deltaTime * 1.0f / rotationDuration * rotateAngle * dir;
                transform.Rotate(0, degrees, 0);
                diff -= (degrees * dir);
                yield return null;
            }

            _audioSource.Stop();
            _rotateCoroutine = null;
            _rotation += (rotateAngle * dir);
            CheckChannels();
        }

        private IEnumerator BlockObject()
        {
            var diff = 0.145f;
            while (diff > 0)
            {
                var translation = Time.deltaTime * 0.145f * 4;
                transform.Translate(0, -translation, 0);
                diff -= translation;
                yield return null;
            }
        }
    }
}