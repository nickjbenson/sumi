// MidiInput.cs

using UnityEngine;
using System.Collections.Generic;

namespace Sumi {

  public enum MidiMessageType {
    NoteOn,
    NoteOff,
    NoteAftertouch
  }

  public struct MidiMessage {
    public byte channel;
    public MidiMessageType messageType;
    public byte midiNote;
    public byte velocity;
  }

  class MidiInput : MonoBehaviour {

    #region Static

    private static Queue<MidiMessage> s_messageQueue = new Queue<MidiMessage>();
    private static MidiInput s_messageConsumer;
    public static MidiInput Consumer {
      get {
        if (s_messageConsumer == null) {
          GameObject obj = new GameObject();
          obj.name = "Sumi Singleton";
          s_messageConsumer = obj.AddComponent<MidiInput>();
        }
        return s_messageConsumer;
      }
    }

    static MidiInput() {
      //RegisterMidiMessageCallback(OnReceiveMidiMessage);
      Debug.LogError("SumiLib not yet implemented.");
    }

//    [DllImport("SumiLib")]
//    private static extern RegisterMidiMessageCallback(float /*TODO FIXME*/ midiMessageCallback);

    private static void OnReceiveMidiMessage(byte[] byteMessage) {
      s_messageQueue.Enqueue(ParseByteMessage(byteMessage));
    }

    ////////////////////////////////
    // Midi Messages: Expected Bytes
    //
    //  Note Messages:
    //    Byte 0: 0bTTTTNNNN
    //    Byte 1: 0b0KKKKKKK
    //    Byte 2: 0b0VVVVVVV (V = 0)
    //  
    //    T: Message Type
    //      1000 = Note Off
    //      1001 = Note On
    //      1010 = Note Aftertouch ("pressure" input on held note)
    //    N: Midi Channel 0...15
    //    K: Midi Note    0...127
    //    V: Velocity     0...127
    //
    // Other message types not yet supported.

    private static MidiMessage ParseByteMessage(byte[] byteMessage) {
      Debug.Log("Midi message parsing not yet implemented.");
      return new MidiMessage(); // TODO: Implement
    }

    public static void RegisterReceiver(MidiReceiver receiver, int channel=0) {
      s_messageConsumer.InstanceRegisterReceiver(receiver, channel);
    }

    #endregion

    private static bool s_hasInstanceAwoken = false;
    void Awake() {
      if (s_hasInstanceAwoken) {
        Debug.LogError("[MidiInput] Only one instance of MidiInput is allowed, and it is constructed automatically.");
      }
    }

    void Update() {
      if (MidiInput.s_messageQueue.Count != 0) {
        MidiMessage message = MidiInput.s_messageQueue.Dequeue();

        List<IMidiReceiver> receivers = _channelReceivers[message.channel];
        for (int i = 0; i < receivers.Count; i++) {
          receivers[i].OnReceiveMessage(message);
        }
      }
    }

    [SerializeField]
    private List<IMidiReceiver>[] _channelReceivers = new List<IMidiReceiver>[16];

    private void InstanceRegisterReceiver(MidiReceiver receiver, int channel) {
      if (_channelReceivers[channel] == null) {
        _channelReceivers[channel] = new List<IMidiReceiver>();
      }
      _channelReceivers[channel].Add(receiver);
    }

  }

}