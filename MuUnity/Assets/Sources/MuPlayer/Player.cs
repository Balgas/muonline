
/* Позиция устаналивается ли без анимации целенаправлено
 * 	coord = MuCoord
 * либо через поиск пути 
 * 	GoTo(MuCoord);
 * 
 * 
 * 
 */

using UnityEngine;
using System.Collections;
using MuGlobal;

namespace MuPlayer {
	
	public class Player : PlayerBody {
		
		public PlayerData Data;
		public PlayerState State;
		public PlayerSound Sound;
		
		//данные для координатам передвижени
		MuCoord[] CurrentPath;
		int CurrentStateOfPath = -1;
		
		public delegate void DelegateStartMoveTo(PlayerData Data);
		public event DelegateStartMoveTo EventStartMoveTo;
		public event DelegateStartMoveTo EventStartMove;
		public event DelegateStartMoveTo EventEndMove;
		
		//первостепенная единоразовая инициализация
		public virtual void Init() {
			InitWorld();
			InitBody();
			
			Data	= gameObject.AddComponent<PlayerData>();
			State	= gameObject.AddComponent<PlayerState>();
			Sound	= gameObject.AddComponent<PlayerSound>();
		}
		
		//устанавливает класс персонажа
		virtual public void SetClass( Util.Player.Class Class ) {
			Data.Class = Class;
			CreateBody(Data.Class);
		}
		
		//устанавливает поворот пользователя
		public void SetDir(float angle) {
			Dir = angle;
			if (!Config.SmoothPlayerRotation)
				body.transform.localRotation = Data.Rotate;
			else {
				StartCoroutine(Data.SmoothRotation(body.transform));
			}
		}
		
		//переменная угла
		float Dir {
			set { Data.isSwim = State.isSwim; Data.Dir = value; }
			get { return Data.Dir; }
		}
		
		//устанавливает пользователя на карту
		virtual public void SetCoord(MuCoord coord) {
			Coord = coord;
			gameObject.transform.localPosition = Data.Position; 
		}
		
		//переменная координат
		MuCoord Coord  {
			set {
				Data.Coord = value; 
				Data.Position = world.map.terrain.GetHeight(value);
				
				//проверяем сейфзону
				State.isSafe = Util.Map.isSafeZone ( world.players.zones[value.x,value.y] );
				
				if (EventStartMoveTo!=null)
					EventStartMoveTo(Data);
				
			}
			get {
				return Data.Coord;
			}
		}
		
		public void SetPath(MuCoord[] path) {
			//подпрограмма шагов
			if (path.Length>0) {
				CurrentPath = path;
				CurrentStateOfPath = -1;
				//если пользователь не движется то запускаем подпрограмму
				if (!State.isMove) StartCoroutine("StepByStep");
			}
		}
		
		protected IEnumerator StepByStep() {
			//если что-то не так, обр
			if (CurrentStateOfPath!=-1 || CurrentPath==null || CurrentPath.Length==0) yield break;
			
			else CurrentStateOfPath = 0;
			
			//длина пути
			int LengthPath = CurrentPath.Length;
			
			State.isMove = true;
			
			if (EventStartMove!=null)
				EventStartMove(Data);
			
			
			//цикл пока путь не пройден
			while (CurrentStateOfPath<LengthPath) {
				
				MuCoord newcoord = CurrentPath[CurrentStateOfPath];

				//устанавливает поворот
				SetDir (Util.Map.GetDirectionFloat(Coord, newcoord));
				//устанавливает координату
				Coord = newcoord;
				
				float Speed = 300;
				
				while(Data.Position!=transform.localPosition) {
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, Data.Position, Time.deltaTime*Speed);
					yield return null;
				}
				
				if (CurrentStateOfPath==-1) CurrentStateOfPath = 0;
				else {
					CurrentStateOfPath++;
				}

				//новая длина пути
				LengthPath = CurrentPath.Length;
				
			}
			
			State.isMove = false;
			SetDir(Data.Dir); //устанавливает поворот после остановки
			
			if (EventEndMove!=null)
				EventEndMove(Data);
			
			
			yield break;
			
		}
		
	}
	
}