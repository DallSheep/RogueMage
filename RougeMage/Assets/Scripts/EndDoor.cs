using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EndDoor : MonoBehaviour
{
    [SerializeField]
    private Transform halfDoorLeftTransform;    //	Left panel of the sliding door
    [SerializeField]
    public Transform halfDoorRightTransform;    //	Right panel of the sliding door

    [SerializeField]
    private float slideDistance = 0.88f;

    private Vector3 leftDoorOpenPosition;
    private Vector3 rightDoorOpenPosition;

    //	Sound Fx
    [SerializeField]
    private AudioClip doorOpeningSoundClip;
    private AudioSource audioSource;
    private int timesPlayed;


    // Use this for initialization
    void Start()
    {
        leftDoorOpenPosition = new Vector3(-slideDistance, 0f, 0f);
        rightDoorOpenPosition = new Vector3(slideDistance, 0f, 0f);

        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator OpenDoor()
    {
        //Sound is only played once when entering collider
        timesPlayed++;

        if (doorOpeningSoundClip != null && timesPlayed <= 1)
        {
            audioSource.PlayOneShot(doorOpeningSoundClip, 0.5F);
        }
        halfDoorLeftTransform.localPosition = leftDoorOpenPosition;
        halfDoorRightTransform.localPosition = rightDoorOpenPosition;

        yield return null;
    }
}

