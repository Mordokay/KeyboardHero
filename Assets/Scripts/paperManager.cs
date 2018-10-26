using UnityEngine;
using System.Collections;

public class paperManager : MonoBehaviour {

    public bool isMoving;
    Vector3 startPos;
    Vector3 endPos;
    float speed;

    void Start()
    {
        isMoving = false;
        speed = 10.0f;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(this.transform.position, endPos, speed * Time.deltaTime);
            if (endPos.Equals(transform.position))
            {
                isMoving = false;
            }
        }
    }

    public void moveUp()
    {
        isMoving = true;
        startPos = this.transform.position;
        endPos = new Vector3(startPos.x, startPos.y + 2.0f, startPos.z);
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2.0f, this.transform.position.z);
    }

    public void moveToNewPage()
    {
        isMoving = true;
        startPos = this.transform.position;
        endPos = new Vector3(startPos.x, startPos.y + 6.0f, startPos.z);
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 6.0f, this.transform.position.z);
    }
}
