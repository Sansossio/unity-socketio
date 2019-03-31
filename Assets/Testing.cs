using UnityEngine;

public class Testing : MonoBehaviour
{
  public int movementSpeed = 1;
  # region UNITY METHODS
  private void Awake()
  {
    DontDestroyOnLoad(this.gameObject);
  }
  void FixedUpdate()
  {
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");

    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

    this.GetComponent<Rigidbody>().AddForce(movement * movementSpeed);
  }
  #endregion UNITY METHODS
}
