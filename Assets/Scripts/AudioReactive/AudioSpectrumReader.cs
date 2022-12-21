using UnityEngine;

/// <summary>
/// Reads audio spectrum data across 8 bands from a the audio source
/// attached to the same GameObject
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioSpectrumReader : MonoBehaviour
{
    private AudioSource _audioSource;

    // array of samples for each side of the stereo image
    private float[] _samplesLeft = new float[512];
    private float[] _samplesRight = new float[512];

    // array of frequency bands
    private static float[] freqBand = new float[8];
    
    // smoothing values for band buffer
    private static float[] _bandBuffer = new float[8];
    private float[] _bufferDecrease = new float[8];

    // initialization values for each band
    private float[] _freqBandHighest = new float[8];
    
    // intensity values for each band
    public static float[] audioBandIntensity = new float[8];
    public static float[] audioBandIntensityBuffer = new float[8];

    // overall amplitude values
    public static float amplitude, amplitudeBuffer;
    private float _amplitudeHighest;

    // variable to initialize highest values
    public float audioProfile = 5;

    public enum Channel {Stereo, Left, Right};
    public Channel channel = new Channel();
    
    /// <summary>
    /// Initializes the Audio Source to pull data from
    /// </summary>
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        SetAudioProfile(audioProfile);
    }
    
    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        MakeBandBuffer();
        CreateAudioBandIntensity();
        GetAmplitude();
    }

    /// <summary>
    /// Initializes the highest value so values are more consistent when audio first starts
    /// rather than starting with a highest value of 0
    /// </summary>
    /// <param name="audioProfile"></param>
    private void SetAudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            _freqBandHighest[i] = audioProfile;
        }
    }

    /// <summary>
    /// Gets the amplitude average of all the bands combined and remaps it
    /// to a value between 0 and 1
    /// </summary>
    private void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++)
        {
            _CurrentAmplitude += audioBandIntensity[i];
            _CurrentAmplitudeBuffer += audioBandIntensityBuffer[i];
        }

        if(_CurrentAmplitude > _amplitudeHighest)
        {
            _amplitudeHighest = _CurrentAmplitude;
        }

        amplitude = _CurrentAmplitude / _amplitudeHighest;
        amplitudeBuffer = _CurrentAmplitudeBuffer / _amplitudeHighest;
    }

    /// <summary>
    /// Calculates intensity for each band relative to the highest value for the band
    /// Creates values in between 0 and 1 for easier use in controlling other variables
    /// </summary>
    private void CreateAudioBandIntensity()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = freqBand[i];
            }

            audioBandIntensity[i] = (freqBand[i] / _freqBandHighest[i]);
            audioBandIntensityBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    /// <summary>
    /// Creates a buffer that smooths out values
    /// </summary>
    private void MakeBandBuffer()
    {
        for (int g = 0; g < 8; ++g)
        {
            if (freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }
            if (freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    /// <summary>
    /// Pulls spectrum data from the samples
    /// </summary>
    private void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman);
    }

    /// <summary>
    /// Separates audio into sonically correct bands with correct frequency ranges and samples.
    /// Calculates average value for each band based on samples.
    /// </summary>
    private void MakeFrequencyBands()
    {
        /* 22050 Hz / 512 samples = 43Hz per sample
         * 
         * band 0: 2 samples = 86Hz: 0 - 86
         * band 1: 4 samples = 172Hz: 87 - 258
         * band 2: 8 samples = 344Hz: 259 - 602
         * band 3: 16 samples = 688Hz: 603 - 1290
         * band 4: 32 samples = 1376Hz: 1291 - 2666
         * band 5: 64 samples = 2752Hz: 2667 - 5418
         * band 6: 128 samples = 5504Hz: 5419 - 10922
         * band 7: 256 samples = 11008Hz: 10923 - 21930
         *
         *Total = 510 -- 2 short of 512 -- can add 2 to band 7 below
         */

        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            /*
            if (i == 7)
            {
                sampleCount += 2;
            }
            */

            for (int j = 0; j < sampleCount; j++)
            {

                if(channel == Channel.Stereo)
                {
                    average += (_samplesLeft[count] + _samplesRight[count]) * (count + 1);
                }
                if(channel == Channel.Left)
                {
                    average += _samplesLeft[count] * (count + 1);
                }
                if (channel == Channel.Right)
                {
                    average += _samplesRight[count] * (count + 1);
                }

                count++;
            }

            average /= count;

            freqBand[i] = average * 10;
        }
    }
}
