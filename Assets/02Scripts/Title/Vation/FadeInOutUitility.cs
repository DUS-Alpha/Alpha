using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class FadeInOutUitility : MonoBehaviour
    {
        /// <summary>
        /// false = In, true = Out
        /// </summary>
        /// <param name="time"></param>
        /// <param name="isFadeOut"></param>
        public void FadeInOut(float time, bool isFadeOut, Image image)
        {
            StartCoroutine(FadeInOutCoroutine(time, isFadeOut, image));
        }

        private IEnumerator FadeInOutCoroutine(float duration, bool isFadeOut, Image image)
        {
            yield return new WaitForSeconds(2f);
            float _t = 0;
            Color _color = image.color;
            while(_t < 1)
            {
                _t += Time.deltaTime / duration;
                if(!isFadeOut) _color.a = _t;
                else _color.a -= _t;

                image.color = _color;
                if (_t >= 1) break;
                yield return null;
            }
        }
    }
}