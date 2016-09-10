using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float rotSpeed;
	private Rigidbody rb;
	public Camera cam;
	public RawImage serious;
	public RawImage happy;
	public float startingHealth = 100;
	public float currentHealth;
	public Slider healthSlider;
	bool isDead;
	bool damaged;
	bool restart;
	private float colTime;
	public float rateOfDecrease;
	public float increasePerCol;
	public Text restartText;
	public Text timer;
	public Text quitText;
	bool wantsEscape;
	public Text instructionText;
	bool begin;


	void Awake() {
		serious.enabled = true;
		happy.enabled = false;

		restartText.text = "";

		currentHealth = startingHealth;
	}


	void Start () {
		rb = GetComponent<Rigidbody> ();
		restart = false;
		wantsEscape = false;

		timer.text = "Time: " + ((int) Time.timeSinceLevelLoad).ToString();
		quitText.enabled = false;
		instructionText.enabled = true;

		instructionText.text = "Help grumpy Bill get happy by bumping into balloons!\n" +
			"Use the arrow keys to move him around\n" +
			"\nPress any key to begin";

		begin = false;
	}


	void FixedUpdate () {

		if (wantsEscape) {
			if (restartText.enabled) {
				restartText.enabled = false;
			}

			if (instructionText.enabled) {
				instructionText.enabled = false;
			}

			quitText.enabled = true; 
			rb.isKinematic = true;

			quitText.text = "Are you sure you want to quit?\nPress 'Y' to quit, any key to continue";

			if (Input.GetKeyDown (KeyCode.Y)) {
				Application.Quit ();
			} else if (Input.anyKeyDown) {
				quitText.enabled = false;
				rb.isKinematic = false;
				wantsEscape = false;
			}
		}
			
		if (!begin) {
			if (Input.anyKeyDown) {
				instructionText.enabled = false;
				begin = true;
			}
		}

		if (restart) {
			if (Input.GetKeyDown (KeyCode.P))
			{
				SceneManager.LoadScene (0);
			}
		}

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = cam.transform.forward * moveVertical;

		Vector3 rotation = new Vector3 (0f, moveHorizontal, 0.0f);

		rb.AddForce (movement * speed);
		rb.AddTorque (rotation * rotSpeed);

		if (Time.time <= colTime + 1f) {
			serious.enabled = false;
			happy.enabled = true;
		} else {
			serious.enabled = true;
			happy.enabled = false;
		}

		currentHealth -= rateOfDecrease;
		healthSlider.value = currentHealth;

		if (currentHealth <= 0) {
			rb.isKinematic = true;

			if (quitText.enabled) {
				quitText.enabled = false;
			}

			if (instructionText.enabled) {
				instructionText.enabled = false;
			}

			restartText.text = "Game Over\nPress 'P' to Procrastinate";
			restart = true;
		}
		timer.text = "Time: " + ((int) Time.timeSinceLevelLoad).ToString();

		if (Input.GetKeyDown (KeyCode.Escape)) {
			wantsEscape = true;
		}
	}


	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Balloon") {
			colTime = Time.time;
			currentHealth += increasePerCol;
			healthSlider.value = currentHealth;
		}
	}


	void instructions() {
		instructionText.text = "Help grumpy Bill get happy by bumping into balloons!\n" +
			"Use the arrow keys to move him around\n" +
			"\nPress any key to begin";

		if (Input.anyKeyDown) {
			instructionText.enabled = false;
			return;
		}
	}
}