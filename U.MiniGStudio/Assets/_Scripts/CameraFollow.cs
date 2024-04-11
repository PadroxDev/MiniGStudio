using MiniGStudio;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Character character;
    private Transform characterTransform;

    [SerializeField] private float leftLimit = -5f;
    [SerializeField] private float rightLimit = 5f;

    private void Awake()
    {
        characterTransform = character.transform;
    }

    private void LateUpdate()
    {
        if (characterTransform != null)
        {
            Vector3 targetPosition = characterTransform.position;
            targetPosition.y = transform.position.y;

            if (characterTransform.position.x < leftLimit)
            {
                targetPosition.x = leftLimit;
            }

            else if (characterTransform.position.x > rightLimit)
            {
                targetPosition.x = rightLimit;
            }


            transform.LookAt(characterTransform.position);
        }
    }

    public void SetLeftLimit(float limit)
    {
        leftLimit = limit;
    }

    public void SetRightLimit(float limit)
    {
        rightLimit = limit;
    }
}
