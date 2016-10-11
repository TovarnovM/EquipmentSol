using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateTests {
    class Program {
        enum Trigger {
            CallDialed,
            HungUp,
            CallConnected,
            LeftMessage,
            PlacedOnHold,
            TakenOffHold,
            PhoneHurledAgainstWall,
            MuteMicrophone,
            UnmuteMicrophone,
            SetVolume
        }

        enum State {
            OffHook,
            Ringing,
            Connected,
            OnHold,
            PhoneDestroyed
        }

        static StateMachine<State,Trigger>.TriggerWithParameters<int> setVolumeTrigger;

        static void Main(string[] args) {
            var phoneCall = new StateMachine<State,Trigger>(State.OffHook);
            setVolumeTrigger = phoneCall.SetTriggerParameters<int>(Trigger.SetVolume);

            phoneCall.Configure(State.OffHook)
                .Permit(Trigger.CallDialed,State.Ringing);

            phoneCall.Configure(State.Ringing)
                .Permit(Trigger.HungUp,State.OffHook)
                .Permit(Trigger.CallConnected,State.Connected);

            phoneCall.Configure(State.Connected)
                .OnEntry(t => StartCallTimer())
                .OnExit(t => StopCallTimer())
                .InternalTransition(Trigger.MuteMicrophone,t => OnMute())
                .InternalTransition(Trigger.UnmuteMicrophone,t => OnUnmute())
                .InternalTransition<int>(setVolumeTrigger,(volume,t) => OnSetVolume(volume))
                .Permit(Trigger.LeftMessage,State.OffHook)
                .Permit(Trigger.HungUp,State.OffHook)
                .Permit(Trigger.PlacedOnHold,State.OnHold);

            phoneCall.Configure(State.OnHold)
                .SubstateOf(State.Connected)
                .Permit(Trigger.TakenOffHold,State.Connected)
                .Permit(Trigger.HungUp,State.OffHook)
                .Permit(Trigger.PhoneHurledAgainstWall,State.PhoneDestroyed);

            
            


            Console.WriteLine("======Print(phoneCall);=======");
            Print(phoneCall);
            Console.WriteLine("======Fire(phoneCall,Trigger.CallDialed);=======");
            Fire(phoneCall,Trigger.CallDialed);
            Print(phoneCall);
            Console.WriteLine("======Fire(phoneCall,Trigger.CallConnected);=======");
            Fire(phoneCall,Trigger.CallConnected);
            Print(phoneCall);
            Console.WriteLine("======SetVolume(phoneCall,2);=======");
            SetVolume(phoneCall,2);
            Print(phoneCall);
            Console.WriteLine("======Fire(phoneCall,Trigger.PlacedOnHold);=======");
            Fire(phoneCall,Trigger.PlacedOnHold);
            Print(phoneCall);
            Console.WriteLine("======Fire(phoneCall,Trigger.MuteMicrophone);=======");
            Fire(phoneCall,Trigger.MuteMicrophone);
            Print(phoneCall);
            Console.WriteLine("======Fire(phoneCall,Trigger.UnmuteMicrophone);=======");
            Fire(phoneCall,Trigger.UnmuteMicrophone);
            Print(phoneCall);
            Console.WriteLine("======Fire(phoneCall,Trigger.TakenOffHold);=======");
            Fire(phoneCall,Trigger.TakenOffHold);
            Print(phoneCall);
            Console.WriteLine("======SetVolume(phoneCall,11);=======");
            SetVolume(phoneCall,11);
            Print(phoneCall);
            Console.WriteLine("======Fire(phoneCall,Trigger.HungUp);=======");
            Fire(phoneCall,Trigger.HungUp);
            Print(phoneCall);

            Console.WriteLine(phoneCall.ToDotGraph());



            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }

        private static void OnSetVolume(int volume) {
            Console.WriteLine("Volume set to " + volume + "!");
        }

        private static void OnUnmute() {
            Console.WriteLine("Microphone unmuted!");
        }

        private static void OnMute() {
            Console.WriteLine("Microphone muted!");
        }


        static void StartCallTimer() {
            Console.WriteLine("[Timer:] Call started at {0}",DateTime.Now);
        }

        static void StopCallTimer() {
            Console.WriteLine("[Timer:] Call ended at {0}",DateTime.Now);
        }

        static void Fire(StateMachine<State,Trigger> phoneCall,Trigger trigger) {
            Console.WriteLine("[Firing:] {0}",trigger);
            phoneCall.Fire(trigger);
        }
        static void SetVolume(StateMachine<State,Trigger> phoneCall,int volume) {
            phoneCall.Fire(setVolumeTrigger,volume);
        }

        static void Print(StateMachine<State,Trigger> phoneCall) {
            Console.WriteLine("[Status:] {0}",phoneCall);
        }
    }
}
