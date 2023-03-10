using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState { idle, rotation, zigzag }
    //[SerializeField]
    //private PlayerState status;
    [SerializeField] private Transform _possibleChild;
    public EnemiNode CurrentNode;

    public bool CanMove { get; set; } = true;

    [SerializeField] private Transform _currentParent => transform.parent;
    //[SerializeField] private status;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool _shouldRotate = false;

    [SerializeField] private UnityEvent OnLooseLife;

    /// <summary>
    /// if node is a rotate node, rotate around parent
    /// </summary>
    //private void Start()
    //{
    //    status = PlayerState.rotation;
    //}
    private void FixedUpdate()
    {
        if (!CanMove) return;
        switch (CurrentNode.status)
        {
            case PlayerState.rotation:

                CurrentNode.transform.RotateAround(CurrentNode.ParentTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
                break;
            case PlayerState.zigzag:
                CurrentNode.zigZag();
                break;
        }
    }

    /// <summary>
    /// On click event we check if we can inject a proxy node
    /// if we can, we now control this new node
    /// if not we lose a life
    /// </summary>
    public void ClickTrigger()
    {
        Debug.Log("On Click -> Try Inject Child");
        if (CurrentNode != null)
        {
            EnemiNode newNode = CurrentNode.TryToInfect();
            if (newNode != null)
            {
                //newNode.ParentTransform = CurrentNode.GripTransform;
                CurrentNode = newNode;
                CurrentNode.used = true;
                rotationSpeed = newNode.RotationSpeed;
                _shouldRotate = true;
                Debug.Log("Injection succesfull");
            }
            else
            {
                Debug.Log("You missed your shot! -> onLooseLife Invoked");
                OnLooseLife?.Invoke();
            }
        }
    }
}
