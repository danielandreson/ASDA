char byteRead;

void setup() {                
// Turn the Serial Protocol ON
  Serial.begin(9600);
  pinMode(2,OUTPUT);
  pinMode(3,OUTPUT);
  pinMode(4,OUTPUT);

}

void loop() {
   while (Serial.available()) {
     /* read the most recent byte */
        byteRead = Serial.read();
    
       //listen for numbers between 0-9
       if(byteRead=='1'){
       Serial.println("asd");
       digitalWrite(2,HIGH);
       digitalWrite(3,HIGH);
       digitalWrite(4,LOW);
       
       }
       if(byteRead=='0'){
       Serial.println("bsd");
       digitalWrite(2,LOW);
       digitalWrite(3,LOW);
       digitalWrite(4,HIGH);
       }

   }
}
