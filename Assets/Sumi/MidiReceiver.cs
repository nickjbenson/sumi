// MidiReceiver.cs

using UnityEngine;
using System;
using UnityEngine.Events;

namespace Sumi {

  [System.Serializable]
  public class NoteOnEvent : UnityEvent<byte /*midiNote*/, byte /*velocity*/> { }
  public class NoteOffEvent : UnityEvent<byte /*midiNote*/, byte /*velocity*/> { }
  public class NoteAftertouchEvent : UnityEvent<byte /*midiNote*/, byte /*velocity*/> { }

  public class MidiReceiver : MonoBehaviour, IMidiReceiver {

    void Start() {
      MidiInput.RegisterReceiver(this);
    }

    public Action<byte, byte> OnNoteOn;
    public Action<byte, byte> OnNoteOff;
    public Action<byte, byte> OnNoteAftertouch;

    // MidiEvent : UnityEvent<unsigned char /* midi note */, unsigned char /* velocity */>
    public NoteOnEvent OnNoteOnEvent;
    public NoteOffEvent OnNoteOffEvent;
    public NoteAftertouchEvent OnNoteAftertouchEvent;

    public void OnReceiveMessage(MidiMessage message) {
      switch (message.messageType) {
        case MidiMessageType.NoteOn:
          OnNoteOn(message.midiNote, message.velocity);
          OnNoteOnEvent.Invoke(message.midiNote, message.velocity);
          break;
        case MidiMessageType.NoteOff:
          OnNoteOff(message.midiNote, message.velocity);
          OnNoteOffEvent.Invoke(message.midiNote, message.velocity);
          break;
        case MidiMessageType.NoteAftertouch:
          OnNoteAftertouch(message.midiNote, message.velocity);
          OnNoteAftertouchEvent.Invoke(message.midiNote, message.velocity);
          break;
      }
    }

  }

}