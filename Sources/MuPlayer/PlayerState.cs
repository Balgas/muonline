
using UnityEngine;
using System.Collections;


namespace MuPlayer {
	
	public class PlayerState : MonoBehaviour {
		
		public enum Action:int {
		 	Move,
			Stay,
			Swim,
			Run,
			StaySwim,
			SwimWithoutHands
		}
		
		public Action state;
		
		private bool _isSafe = false;
		private bool _isMove = false;
		private Util.Map.Location _map;
		
		private int _swimCount = 0;
		
		public bool isSwim {
			get { return (state == Action.Swim || state == Action.SwimWithoutHands); }	
		}
		
		public bool isSwimZone {
			set; get;	
		}
		
		public Util.Map.Location map {
			set { 
				_map = value;
				isSwimZone = Util.Map.isSwimZone(value);
			} get { return _map; }	
		}
		
		public bool isMove {
			set {
				//если есть изменения
				if (value!=_isMove) { _isMove = value;
					//если движение начато
					if (value) Play(Action.Move);
					//если движение остановлено
					else Play(Action.Stay);
				}
			}
			get { return _isMove; }
		}
		
		public bool isSafe {
			set {
				//если есть изменения
				if (value!=_isSafe) { _isSafe = value; if (isMove) Play(Action.Move); } 
			}
			get { return _isSafe; }
		}
		
		//вариации перед проигрываением анимации
		public Action ApplyState(Action state) {
			switch (state) {
				case Action.Move: //варианты движения
					if (isSafe) { //если безопасная зона
						state = Action.Move;
					} else { //если не безопасная зона
						if (isSwimZone) { //если зона для плавания
							state = Action.SwimWithoutHands;
						}
					}
				break;
				case Action.Stay:
					if (isSwimZone && !isSafe) { //если остановился в батл зоне Атланаса
						state = Action.StaySwim;
					}
				break;
			}
			return state;
		}
		
		public void Play ( Action state )	{ SetState(state); }
		
		void SetState(Action state) {
			this.state = ApplyState(state);
			PlayerDetail[] details = gameObject.GetComponentsInChildren<PlayerDetail>();
			foreach (PlayerDetail detail in details) {
				detail.Play(this.state.ToString());
			}
		}
	}
	
}