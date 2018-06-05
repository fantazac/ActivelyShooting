using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D hitbox;

    private float disableDuration;
    private WaitForSeconds disableDelay;

    private PlatformManager()
    {
        disableDuration = 0.6f;
        disableDelay = new WaitForSeconds(disableDuration);
    }

    public void JumpingDown()
    {
        hitbox.enabled = false;
        StartCoroutine(EnablePlatform());
    }

    private IEnumerator EnablePlatform()
    {
        yield return disableDelay;

        hitbox.enabled = true;
    }
}
