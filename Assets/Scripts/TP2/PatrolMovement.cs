using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolMovement : MonoBehaviour
{
   public enum MoveMethod
   {
      MoveTowards,
      Lerp,
      SmoothDamp,
      Custom
   }

   [SerializeField] private Transform _borne1;
   [SerializeField] private Transform _borne2;
   [SerializeField] private MoveMethod _moveMethod;
   [SerializeField] private float _speed = 5f;
   
   [SerializeField] private float _minDistanceThreshold = 0.2f;
   private Transform _nextBorne;

   [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

   
   private void Start()
   {
      _nextBorne = _borne1;
   }
   

   private void Update()
   {
      var distance = Vector3.Distance(_borne1.position, _borne2.position);
      var neededTime = distance / _speed;
      
      switch (_moveMethod)
      {
         case MoveMethod.MoveTowards:
            transform.position = Vector3.MoveTowards(transform.position, _nextBorne.position, _speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _nextBorne.position) < _minDistanceThreshold)
               _nextBorne = _nextBorne == _borne1 ? _borne2 : _borne1;
            break;
         case MoveMethod.Lerp:
            transform.position = Vector3.Lerp(_borne1.position, _borne2.position, Mathf.PingPong(Time.time / neededTime, 1));
            break;
         case MoveMethod.SmoothDamp:
            transform.position = Vector3.Lerp(_borne1.position, _borne2.position, Mathf.SmoothStep(0, 1, Mathf.PingPong(Time.time / neededTime, 1)));
            break;
         case MoveMethod.Custom:
            transform.position = Vector3.Lerp(_borne1.position, _borne2.position, _animationCurve.Evaluate(Mathf.PingPong(Time.time / neededTime, 1)));
            break;
      }
   }
}
