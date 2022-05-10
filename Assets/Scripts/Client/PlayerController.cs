#if !SERVER_BUILD || UNITY_EDITOR
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    // Client-only serialized fields, also accessible in editor.
    public Animator visualPrefab;
    public string horizontalAxis;
    public string verticalAxis;

    // Client-only methods and unserialized data.
    // We don't use the || UNITY_EDITOR
    // exception here, to ensure we don't conflict with the
    // server file's method definitions.

    #if !SERVER_BUILD    
    Animator _visual;
    Camera _camera;

    void OnStartup()
    {
        _visual = Instantiate(visualPrefab, transform);
        _camera = Camera.main;
    }

    void Update()
    {
        // Capture input from client's controller.
        Vector2 input = new Vector2(Input.GetAxis(horizontalAxis), 
                                    Input.GetAxis(verticalAxis));

        // TODO: SendInputToServer(input);        

        // Locally predict the movement based on this input.
        Move(input);

        // Update our animation states accordingly.
        _visual.SetFloat("horizontal", input.x);
        _visual.SetFloat("vertical", input.y);

        // TODO: Handle applying corrections from server in case of bad predictions.
    }
    #endif
}
#endif