using UnityEngine;
using System.Collections;

public class ball_script : MonoBehaviour 
{
    private AudioSource audioSource_component;
    private Animator animator_Component;

    private float actualBlendValue = 0.0f;
    public float RotateSpeed = 10f;

    //Sons
    public AudioClip[] BoingClips;
    public AudioClip[] HumClips;
    public AudioClip[] ExplodeClips;
    public AudioClip[] ScratchClips;

    private bool ballIdle = false;
    
	void Start ()
	{
	    animator_Component = gameObject.GetComponent<Animator>();
	    audioSource_component = gameObject.AddComponent<AudioSource>();
	}

    void Update ()
	{
        if (Input.GetKeyUp(KeyCode.Space)
            && !animator_Component.GetCurrentAnimatorStateInfo(0).IsName("explode_state"))
        {
            animator_Component.SetTrigger("explode_trigger");
            actualBlendValue = 0.0f;
        }


        if (Input.GetKeyUp(KeyCode.Return)
            && !animator_Component.GetCurrentAnimatorStateInfo(0).IsName("scratch_state"))
        {
            animator_Component.SetTrigger("scratch_trigger");
            actualBlendValue = 0.0f;
        }

        if (!animator_Component.GetCurrentAnimatorStateInfo(0).IsName("explode_state")
            && !animator_Component.GetCurrentAnimatorStateInfo(0).IsName("scratch_state"))
        {
            animator_Component.SetTrigger("blendtree_trigger");


            float toBlendValue = 0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                toBlendValue = 1f;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                toBlendValue = -1f;
            }

            actualBlendValue = floatTransition(actualBlendValue, toBlendValue, 0.1f);

            if (actualBlendValue < 0.01f && toBlendValue > -0.01f)
                actualBlendValue = 0f;

            if (actualBlendValue > 0.99f)
                actualBlendValue = 1f;

            if (actualBlendValue < -0.99f)
                actualBlendValue = -1f;


            if (actualBlendValue == 0f)
                ballIdle = true;
            else
            {
                ballIdle = false;
            }

            float rotationDir = 0f;

            if (Input.GetKey(KeyCode.LeftArrow))
                rotationDir = 1f;


            if (Input.GetKey(KeyCode.RightArrow))
                rotationDir = -1f;

            transform.Rotate(0, rotationDir*RotateSpeed*Time.deltaTime, 0, Space.Self);
        }

        animator_Component.SetFloat("Blend", actualBlendValue);


	}

    // function to make interpolation inbetween 2 float values, determiined by a duration
    private float floatTransition(float from, float to, float duration)
    {
        return Mathf.Lerp(from, to, Time.deltaTime / duration);
    }

    // function tu play a random audioclips attached to this gameobject
    private void PlayRandomClip(AudioClip[] audioClips, float pitch)
    {
        if (audioClips.Length > 0)
        {
            audioSource_component.Stop();
            int clipID = Mathf.RoundToInt(Random.Range(0.0f, (float)audioClips.Length - 1.0f));
            audioSource_component.clip = audioClips[clipID];
            audioSource_component.pitch = pitch;
            audioSource_component.Play();
        }
    }

    public void PlayBoingSound()
    {
        if(!ballIdle)
            PlayRandomClip(BoingClips,1f);
    }

    public void PlayHumSound()
    {
        if (ballIdle && !audioSource_component.isPlaying)
            PlayRandomClip(HumClips, 1f);
    }

    public void PlayExplodeSound()
    {
        if(!audioSource_component.isPlaying)
            PlayRandomClip(ExplodeClips,1f);
    }

    public void PlayScratchSound()
    {
        if(!audioSource_component.isPlaying)
            PlayRandomClip(ScratchClips,1f);
    }
}
