#include <MsTimer2.h>

#define BUF_LEN 50
String VERSION = String("KHTimer v1.00");
const char TERMINATOR = ';';

// DP, F, G, A, B, C, D, E
int DIGITS[11] = {
  B01011111, // 0 - 
  B00001100, // 1 -
  B00111011, // 2 -
  B00111110, // 3 -
  B01101100, // 4 -
  B01110110, // 5 -
  B01100111, // 6 -
  B00011100, // 7 -
  B01111111, // 8 -
  B01111100, // 9 -
  B00000000  // off
};
int POSITIONS[4] = {A0, A1, A2, A3};

const int ZERO = 10;

String buff;
volatile bool timerGo = true;
volatile int timerDigits[4] = {0,0,0,0};

void setup() {
  pinMode(A0,OUTPUT);
  pinMode(A1,OUTPUT);
  pinMode(A2,OUTPUT);
  pinMode(A3,OUTPUT);
  pinMode(A5,OUTPUT);
  pinMode(13,OUTPUT);

  pinMode(2,OUTPUT);
  pinMode(3,OUTPUT);
  pinMode(4,OUTPUT);
  pinMode(5,OUTPUT);
  pinMode(6,OUTPUT);
  pinMode(7,OUTPUT);
  pinMode(8,OUTPUT);
  pinMode(9,OUTPUT);

  digitalWrite(A0,LOW);
  digitalWrite(A1,LOW);
  digitalWrite(A2,LOW);
  digitalWrite(A3,LOW);

  WriteDigit(ZERO);
  
  digitalWrite(A5,LOW);
  
  // start serial port at 9600 bps and wait for port to open:
  Serial.begin(9600, SERIAL_8N1);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  SendResponse("");
  SendResponse("");
  SendResponse(VERSION);

  MsTimer2::set(1, OnTimer); // 1ms period
  MsTimer2::start();
}

void loop() {
  if (Serial.available() > 0)
  {
    buff = ReadCommand();
    buff.trim();
    int idx = buff.indexOf(':');
    if (idx < 0) 
    {
      // if there is no ':' this is not the right command
      return;
    } 
    else if (idx == 0)
    {
      // addressed to all devices
    }
    else if (idx > 0)
    {
      String num = buff.substring(0, idx + 1);
      if (0 != num.toInt())
      {
        // not address of this device
        return;
      }
    }
    buff = buff.substring(idx + 1);
    idx = buff.indexOf(':');
    if (idx < 0) 
    {
      OnUnknownCommand(buff);
      return;
    }
    buff = buff.substring(idx + 1);
    if (buff.length() > 0)
    {
      if (buff.startsWith("ver"))
      {
        SendResponse(VERSION);
      }
      else if (buff.startsWith("time"))
      {
        String tmp = String(timerGo ? "run" : "stop") + ":" + String(timerDigits[0],DEC) + String(timerDigits[1],DEC) + '.' + String(timerDigits[2],DEC) + String(timerDigits[3],DEC);
        SendResponse(tmp);
      }
      else if (buff.startsWith("stop"))
      {
        timerGo = false;
        SendResponse("OK");
      }
      else if (buff.startsWith("start"))
      {
        timerGo = true;
        SendResponse("OK");
      }
      else if (buff.startsWith("reset"))
      {
        timerGo = false;
        timerDigits[0] = 0;
        timerDigits[1] = 0;
        timerDigits[2] = 0;
        timerDigits[3] = 0;
        timerGo = true;
        SendResponse("OK");
      }
      else if (buff.startsWith("set"))
      {
        idx = buff.indexOf(':');
        buff = buff.substring(idx + 1);
        if (buff.length() >= 4)
        {
          timerGo = false;
          SetTime(0, buff[0] - 0x30);
          SetTime(1, buff[1] - 0x30);
          SetTime(2, buff[2] - 0x30);
          SetTime(3, buff[3] - 0x30);
          timerGo = true;
        }
        SendResponse("OK");
      }
      else if (buff.startsWith("stat"))
      {
        SendResponse(timerGo ? "run" : "stop");
      }
      else if (buff.startsWith("help"))
      {
        SendResponse("ver,time,stop,start,reset,stat");
      }
      else
      {
        OnUnknownCommand(buff);
      }
    }
  }
}

void SetTime(int pos, int val)
{
  if (pos >= 4) return;
  val = val > 9 ? 0 : val;
  timerDigits[pos] = val;
}

void OnUnknownCommand(String buff)
{
  SendResponse("Unknown command: '" + buff + "'!", 1);
}

int counter = 0;
void OnTimer() {
  
  {
    // show current LED
    int digit = counter & 0x3;
    DisplayOff();
    WriteDigit(timerDigits[digit]);
    ShowDigit(digit);
  }
  if (counter < 500)
  {
    counter++;
    return;
  }

  counter = 0;
  static boolean output = HIGH;
  digitalWrite(13, output);
  digitalWrite(9, output && timerGo);
  output = !output;
  if (output)
  {
    if (timerGo)
    {
      SecondUp();
    }
  }
}

String ReadCommand()
{
  digitalWrite(A5,LOW);
  delay(2);
  String tmp = Serial.readStringUntil(TERMINATOR);
  return tmp;
}

void SendResponse(const String& data)
{
  SendResponse(data, 0);
}

void SendResponse(const String& data, int rv)
{
  int tmp = 0;
  for (int i=0; i < data.length(); i++)
  {
    tmp += data[i];
  }
  tmp = tmp & 0xff;
  SendResponseInt(String("0:") + String(rv) + ':' + String(tmp,HEX) + ':' + data);
}

void SendResponseInt(const String& data)
{
  digitalWrite(A5,HIGH);
  delay(2);
  if (data.length() == 0)
  {
    Serial.println(data);
  }
  else
  {
    Serial.println(data + ";");
  }
  Serial.flush();
  delay(2);
  digitalWrite(A5,LOW);
}

void SecondUp()
{
  // seconds
  int tmp = timerDigits[3];
  tmp++;
  if (tmp < 10)
  {
    timerDigits[3] = tmp;
    return;
  }
  timerDigits[3] = 0;
  
  // 10th seconds
  tmp = timerDigits[2];
  tmp++;
  if (tmp < 6)
  {
    timerDigits[2] = tmp;
    return;
  }  
  timerDigits[2] = 0;

  // minutes
  tmp = timerDigits[1];
  tmp++;
  if (tmp < 10)
  {
    timerDigits[1] = tmp;
    return;
  }  
  timerDigits[1] = 0;

  // 10th minutes
  tmp = timerDigits[0];
  tmp++;
  if (tmp < 10)
  {
    timerDigits[0] = tmp;
    return;
  }  
  timerDigits[0] = 0;  
}

void DisplayOff()
{
  for (int i=0; i < 4; i++)
  {
    digitalWrite(POSITIONS[i],LOW);
  }
}

void ShowDigit(int digit)
{
  digitalWrite(POSITIONS[digit],HIGH);
}

void WriteDigit(int digit)
{
  int d = DIGITS[digit];
  //int d = B00000010;
  digitalWrite(8,d & 1); //e
  d = d >> 1;
  digitalWrite(7,d & 1); //d
  d = d >> 1;
  digitalWrite(6,d & 1); //c
  d = d >> 1;
  digitalWrite(5,d & 1); //b
  d = d >> 1;
  digitalWrite(4,d & 1); //a
  d = d >> 1;
  digitalWrite(3,d & 1); //g
  d = d >> 1;
  digitalWrite(2,d & 1); //f
}

