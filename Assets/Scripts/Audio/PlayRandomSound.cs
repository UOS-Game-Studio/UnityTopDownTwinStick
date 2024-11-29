using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class PlayRandomSound : MonoBehaviour
    {
        public AudioResource audioResource;
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlaySound()
        {
            _source.resource = audioResource;
            _source.Play();
        }
    }
}
