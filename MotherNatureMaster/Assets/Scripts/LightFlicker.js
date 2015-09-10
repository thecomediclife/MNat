#pragma strict

var minFlickerIntensity : float = 0.5;
 var maxFlickerIntensity : float = 2.5;
 var flickerSpeed : float = 0.035;
 
 private var randomizer : int = 0;
  
  while (true)
  {
     if (randomizer == 0) {
       GetComponent.<Light>().intensity = (Random.Range (minFlickerIntensity, maxFlickerIntensity));
 
     }
     else GetComponent.<Light>().intensity = (Random.Range (minFlickerIntensity, maxFlickerIntensity));
 
     randomizer = Random.Range (0, 1.1);
     yield WaitForSeconds (flickerSpeed);
 }