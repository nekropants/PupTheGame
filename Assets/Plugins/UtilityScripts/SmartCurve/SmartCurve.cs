// using System;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Serialization;
// using Random = UnityEngine.Random;
//
// namespace NightTools.Utility
// {
//     [Serializable]
//     public class SmartCurve
//     {
//         private float _timer;
//
//         [SerializeField] private AnimationCurve _curve = AnimationCurve.Constant(0, 1, 1);
//         [SerializeField] private float _frequencyM = 1f;
//         [SerializeField] private float _amplitudeM = 1f;
//         [SerializeField] private float _phaseShift = 0f;
//         [SerializeField] private float _normalizedSample;
//         [SerializeField] private float _lastSampleTime = 0.0f;
//         [SerializeField] private float _lastResult = 0.0f;
//
//         public SmartCurve(AnimationCurve curve)
//         {
//             _curve = curve;
//         }
//
//         public SmartCurve(float constant)
//         {
//             _curve =  AnimationCurve.Constant(0f, 1f, constant);
//         }
//
//         public SmartCurve(float start, float end)
//         {
//             _curve =  AnimationCurve.Linear(0f, start, 1f,end);
//         }
//
//         public AnimationCurve curve => _curve;
//
//         public float amplitudeM => _amplitudeM;
//
//         public float frequencyM => _frequencyM;
//     
//         public float phaseShift => _phaseShift;
//     
//         public void UpdateTimerDt()
//         {
//             _timer += Time.deltaTime;
//         }
//
//         public void ResetTimer()
//         {
//             _timer = 0;
//         }
//
//         public float Evaluate()
//         {
//             return Evaluate(_timer);
//         }
//
//         public float Evaluate(float t)
//         {
//              _lastSampleTime = t * frequencyM + phaseShift;
//
//              float duration = 0;
//              if (curve.length > 0)
//              {
//                  duration = curve.keys[curve.keys.Length - 1].time;
//              }
//              _normalizedSample = _lastSampleTime / duration;
//             _lastResult = curve.Evaluate(_lastSampleTime);
//
//             return _lastResult * amplitudeM;
//         }
//
//         public float EvaluateRawCurve()
//         {
//             return EvaluateRawCurve(_timer);
//         }
//
//         public float EvaluateRawCurve(float t)
//         {
//             return curve.Evaluate(t);
//         }
//         
//         public float RandomSample()
//         {
//             float t = Random.value * curveDistance + phaseShift;
//             return curve.Evaluate(t)* Random.value*amplitudeM;
//         }
//
//         private float curveDistance => curve.keys[curve.keys.Length -1].time;
//
//         public void Normalize()
//         {
//             float  furthest = curve.keys[curve.length - 1].time;
//             float highest = float.MinValue;
//             
//             for (int i = 0; i < curve.length; i++)
//             {
//                 Keyframe key = curve.keys[i];
//                 highest = Mathf.Max(key.value, highest);
//             }
//             
//             
//             for (int i = curve.length - 1; i >= 0; i--)
//             {
//                 Keyframe key = curve.keys[i];
//                 key.time /= furthest;
//                 key.value /= highest;
//                 
//                 curve.RemoveKey(i);
//                 curve.AddKey(  key.time ,    key.value);
//             }
//
//             _frequencyM = 1 / furthest;
//             _amplitudeM= highest;
//             
//         }
//
//         public void Freeze()
//         {
//             for (int i = curve.length - 1; i >= 0; i--)
//             {
//                 Keyframe key = curve.keys[i];
//                 key.time /= frequencyM;
//                 key.time += phaseShift;
//                 key.value *= amplitudeM;
//                 
//                 curve.RemoveKey(i);
//                 curve.AddKey(  key);
//             }
//
//             _frequencyM = 1;
//             _amplitudeM= 1;
//             _phaseShift = 0;
//         }
//     }
// }