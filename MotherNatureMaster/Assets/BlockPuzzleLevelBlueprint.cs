using UnityEngine;
using System.Collections;

public class BlockPuzzleLevelBlueprint : MonoBehaviour {
	//THIS SCRIPT IS FOR LEVEL 7, THE BLOCK PUZZLE LEVEL.
	public enum PuzzleLayer{Layer1, Layer2, Layer3, Layer4};
	public PuzzleLayer currentPuzzle = PuzzleLayer.Layer1;

	public Transform[] currentBlocks, trees, lights, cylinders = new Transform[8];
	public Transform[] blockLayer1, blockLayer2, blockLayer3, blockLayer4 = new Transform[8];
	public Transform[] blockLayers = new Transform[4];
	public Transform[] icons = new Transform[5];

	public Transform wall;
	public Transform key;
	public Transform nullCube;

	public Material greyMat, greenMat, whiteMat, redMat;

	public bool resetting;
	public bool puzzleCompleted;

	private float wallHeight = 17.5f;

	// Update is called once per frame
	void Update () {
		if (!resetting) {
			switch (currentPuzzle) {

			case PuzzleLayer.Layer1:
				EnableLayer (blockLayer1);
				AddBlockForce (blockLayer1);

				break;

			case PuzzleLayer.Layer2:
				EnableLayer (blockLayer2);
				AddBlockForce (blockLayer2);

				break;

			case PuzzleLayer.Layer3:
				EnableLayer (blockLayer3);
				AddBlockForce (blockLayer3);

				break;

			case PuzzleLayer.Layer4:
				EnableLayer (blockLayer4);
				AddBlockForce (blockLayer4);

				break;
			}
		} else {
			bool finishedReset = true;
			for (int i = 0; i < trees.Length; i++) {
				trees[i].GetComponent<TreeController6>().Decay();
				if (!trees[i].GetComponent<TreeController6>().onGround)
					finishedReset = false;
			}

			wall.transform.localPosition = Vector3.MoveTowards(wall.transform.localPosition, new Vector3(0f,wallHeight,0f), 0.5f);

			switch (currentPuzzle) {
			case PuzzleLayer.Layer1:
				Debug.Log ("should never happen");
				break;

			case PuzzleLayer.Layer2:
				VanishBlocks(blockLayer1, 0);
				break;

			case PuzzleLayer.Layer3:
				VanishBlocks(blockLayer2, 1);
				break;

			case PuzzleLayer.Layer4:
				VanishBlocks(blockLayer3, 2);
				break;
			}


			if (finishedReset) {
				resetting = false;
			}
		}

		////////////////////////////////Puzzle has been completed
		if (puzzleCompleted) {
			for (int i = 0; i < trees.Length; i++) {
				trees[i].GetComponent<TreeController6>().Decay();
			}
			VanishBlocks(blockLayer4, 3);
		}

		MatchCylinderHeight();

		if (Mathf.Abs(key.transform.position.y - 18.5f) < 0.1f && currentPuzzle == PuzzleLayer.Layer1) {
			resetting = true;
			currentPuzzle = PuzzleLayer.Layer2;
			icons[1].GetComponent<Renderer>().material = redMat;
			wallHeight -= 1.0f;
		}
		if (Mathf.Abs(key.transform.position.y - 17.5f) < 0.1f && currentPuzzle == PuzzleLayer.Layer2) {
			resetting = true;
			currentPuzzle = PuzzleLayer.Layer3;
			icons[2].GetComponent<Renderer>().material = redMat;
			wallHeight -= 1.0f;
		}
		if (Mathf.Abs(key.transform.position.y - 16.5f) < 0.1f && currentPuzzle == PuzzleLayer.Layer3) {
			resetting = true;
			currentPuzzle = PuzzleLayer.Layer4;
			icons[3].GetComponent<Renderer>().material = redMat;
			wallHeight -= 1.0f;
		}
		if (Mathf.Abs(key.transform.position.y - 14.5f) < 0.1f && currentPuzzle == PuzzleLayer.Layer4) {
			puzzleCompleted = true;
			icons[4].GetComponent<Renderer>().material = redMat;
		}

	}

	void EnableLayer(Transform[] blockLayer) {
		for (int i = 0; i < blockLayer.Length; i++) {
			if (blockLayer[i] == nullCube) {
				trees[i].GetComponent<TreeController6>().maxHeightY = 2.0f;
				lights[i].GetComponent<Renderer>().material = greyMat;
			} else {
				trees[i].GetComponent<TreeController6>().maxHeightY = 3.5f;
				lights[i].GetComponent<Renderer>().material = greenMat;
			}
		}
	}


	//This function makes sure the cylinder height is correct based on the height of the platform of the tree. Never goes below the 12f y value.
	void MatchCylinderHeight() {
		for (int i = 0; i < trees.Length; i++) {
			//Vector3 cylPos = cylinders[i].localPosition;
			Vector3 treePos = trees[i].transform.GetChild(0).transform.localPosition;
			float yPos = treePos.y + 10f;
			if (yPos < 12f) {
				yPos = 12f;
			}

			cylinders[i].localPosition = new Vector3(cylinders[i].localPosition.x, yPos, cylinders[i].localPosition.z);
		}
	}

	//This Function pushes the blocks according to the height of the cylinders.
	void AddBlockForce(Transform[] blockLayer) {
		for (int i = 0; i < blockLayer.Length; i++) {
			if (blockLayer[i] != nullCube) {
				if (Mathf.Approximately(cylinders[i].localPosition.y,13.5f)) {
					if (i < 4) {
						blockLayer[i].GetComponent<Rigidbody>().AddForce(-10f,0f,0f);
					} else {
						blockLayer[i].GetComponent<Rigidbody>().AddForce(0f,0f,-10f);
					}
				} else if (Mathf.Approximately(cylinders[i].localPosition.y,12f)) {
					if (i < 4) {
						blockLayer[i].GetComponent<Rigidbody>().AddForce(10f,0f,0f);
					} else {
						blockLayer[i].GetComponent<Rigidbody>().AddForce(0f,0f,10f);
					}
				}
			}
		}
	}

	//This function vanishes the blocks after the puzzle is completed;
	void VanishBlocks(Transform[] blockLayer, int index) {
		for (int i = 0; i < blockLayer.Length; i++) {
			if (blockLayer[i] != nullCube) {
				float alph = blockLayer[i].GetComponent<Renderer>().material.color.a;
				alph -= 0.025f;

				if (alph < 0f) {
					blockLayers[index].gameObject.SetActive(false);
				}

				blockLayer[i].GetComponent<Renderer>().material.color = new Color(0f,0f,0f,alph);
			}
		}
	}
}
