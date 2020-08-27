using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public class CameraController : MonoBehaviour
    {
        private CameraModel _cameraModel;
        private float _currentRadius;
        private Transform _radiusPivot;
        private Transform _cameraPivot;
        private Camera _camera;
        private float _currentFov;

        public void Initialize(CameraModel cameraModel)
        {
            if (cameraModel == null) return;

            _cameraModel = cameraModel;

            _radiusPivot = transform.GetChild(0);
            _radiusPivot.localPosition = Vector3.right * _cameraModel.roundRadius;
            _currentRadius = _cameraModel.roundRadius;

            _camera = Camera.main;
            _cameraPivot = _camera.transform;
            _cameraPivot.localPosition = Vector3.up * _cameraModel.height;
            _cameraPivot.LookAt(Vector3.up * _cameraModel.lookAtHeight);
            _currentFov = (_cameraModel.fovMin + _cameraModel.fovMax) / 2;

            StartCoroutine(RadiusChanger());
            StartCoroutine(FovChanger());
        }

        private IEnumerator RadiusChanger()
        {
            while (true)
            {
                yield return new WaitForSeconds(_cameraModel.roamingDuration);
                _currentRadius = _cameraModel.roundRadius + Random.Range(-1, 2);
            }
        }

        private IEnumerator FovChanger()
        {
            while (true)
            {
                yield return new WaitForSeconds(_cameraModel.fovDelay);
                _currentFov = Random.Range(_cameraModel.fovMin, _cameraModel.fovMax);
            }
        }

        private void Update()
        {
            if (_cameraModel == null) return;
            
            transform.Rotate(Vector3.up, (360 * Time.deltaTime) / _cameraModel.roundDuration);

            _radiusPivot.localPosition = Vector3.Lerp(_radiusPivot.localPosition, Vector3.right * _currentRadius,
                Time.deltaTime / _cameraModel.roamingDuration);

            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _currentFov,
                Time.deltaTime / _cameraModel.fovDuration);
        }
    }
}