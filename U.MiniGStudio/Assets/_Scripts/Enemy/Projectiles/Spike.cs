using UnityEngine;

namespace MiniGStudio
{

    public class Spike : MonoBehaviour
    {
        private enum State
        {
            Rising,
            Waiting,
            Hiding
        }

        public GolemGroundSmashState.Descriptor _desc;

        private Vector3 _start;
        private Vector3 _dest;

        private float _elapsedTime;

        private State currentState;

        public void Start()
        {
            _elapsedTime = 0.0f;
            _start = transform.position;
            _dest = transform.position + Vector3.up * (_desc.targetHeight - _desc.initialHeight);
            currentState = State.Rising;
        }

        private void Update()
        {
            
            _elapsedTime += Time.deltaTime;

            switch (currentState)
            {
                case State.Rising:
                    transform.position = Vector3.Lerp(_start, _dest, _elapsedTime / _desc.riseDelay);
                    if (_elapsedTime >= _desc.riseDelay)
                    {
                        currentState = State.Waiting;
                        _elapsedTime = 0.0f;
                    }
                    break;
                case State.Waiting:
                    if (_elapsedTime >= _desc.waitDelay)
                    {
                        currentState = State.Hiding;
                        _elapsedTime = 0.0f;
                    }
                    break; 
                case State.Hiding:
                    transform.position = Vector3.Lerp(_dest, _start, _elapsedTime / _desc.hideDelay);
                    if (_elapsedTime >= _desc.riseDelay)
                    {
                        Destroy(gameObject);
                    }
                    break;
            }

            

        }
    } 
}
