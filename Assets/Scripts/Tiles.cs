using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{  
    public Vector3 targetPosition;
    [SerializeField]
    private Vector3 correctPosition;
    private SpriteRenderer _sprite;
    [SerializeField]
    public int position;
    public Sprite newSprite;
    public Sprite oldSprite;
    [SerializeField]
    private float lerpAmount;

    public bool inRightPlace = false;
    // Start is called before the first frame update
    void Awake()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpAmount);

        if (gameObject.tag == "KeyObject")
        {
            if(targetPosition == correctPosition)
            {
                ChangeSprite();
                
            }
            else
            {
                WrongSprite();
                
            }
        }
        else
        {
            inRightPlace = false;
        }
        
    }

    private void ChangeSprite()
    {
        _sprite.sprite = newSprite;
        inRightPlace = true;
    }

    private void WrongSprite()
    {
        _sprite.sprite = oldSprite;
        inRightPlace = false;
    }
}
