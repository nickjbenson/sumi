// IMidiReceiver.cs

namespace Sumi {
  public interface IMidiReceiver {
    void OnReceiveMessage(MidiMessage message);
  }
}