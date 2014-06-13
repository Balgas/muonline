
using UnityEngine;
using System.Collections;
using MuPlayer;
using System;

public class PathFinder : MonoBehaviour {
	
	
	
	public static MuCoord[] Get(MuCoord start, MuCoord end, byte[,] zones) {
		return FindPath(start.x, start.y, end.x, end.y, zones);
	}
	
	static MuCoord[] FindPath (int startX, int startY, int targetX, int targetY, byte[,] zones) {
		int mapWidth = Config.MapLength, mapHeight = Config.MapLength;
		int onClosedList = 10;
		int found = 1, nonexistent = 2; 
		int walkable = 0, unwalkable = 1;
	
		int[] openList = new int[mapWidth*mapHeight+2]; //1 dimensional array holding ID# of open list items
		int[,] whichList = new int[mapWidth+1,mapHeight+1];  //2 dimensional array used to record 
	
		int[] openX = new int[mapWidth*mapHeight+2]; //1d array stores the x location of an item on the open list
		int[] openY = new int[mapWidth*mapHeight+2]; //1d array stores the y location of an item on the open list
		int[,] parentX = new int[mapWidth+1,mapHeight+1]; //2d array to store parent of each cell (x)
		int[,] parentY = new int[mapWidth+1,mapHeight+1]; //2d array to store parent of each cell (y)
		int[] Fcost = new int[mapWidth*mapHeight+2];	//1d array to store F cost of a cell on the open list
		int[,] Gcost = new int[mapWidth+1,mapHeight+1]; //2d array to store G cost for each cell.
		int[] Hcost = new int[mapWidth*mapHeight+2];	//1d array to store H cost of a cell on the open list
		
		int onOpenList=0, parentXval=0, parentYval=0,
		a=0, b=0, m=0, u=0, v=0, temp=0, corner=0, numberOfOpenListItems=0,
		addedGCost=0, tempGcost = 0, path = 0, x=0, y=0, pathX, pathY, newOpenListItemID=0;
		
		//если позиция та на которой стоишь
		if (startX == targetX && startY == targetY)
			return new MuCoord[0];
		
				
		//если туда нельзя пройти
		if (!isWay (zones[targetX,targetY])) {
			Debug.Log("zone: "+zones[targetX,targetY]);
			return new MuCoord[0];
		}

		for (x = 0; x < mapWidth; x++) {
			for (y = 0; y < mapHeight;y++)
				whichList [x,y] = 0;
		}
		onClosedList = 2; 
		onOpenList = 1;
		Gcost[startX,startY] = 0;

		numberOfOpenListItems = 1;
		openList[1] = 1;
		openX[1] = startX ; openY[1] = startY;
		do {
			if (numberOfOpenListItems != 0) {

				parentXval = openX[openList[1]];
				parentYval = openY[openList[1]];
				whichList[parentXval,parentYval] = onClosedList;


				numberOfOpenListItems = numberOfOpenListItems - 1;
		
				openList[1] = openList[numberOfOpenListItems+1];
				v = 1;

				do {
					u = v;		
					if (2*u+1 <= numberOfOpenListItems) {
	
						if (Fcost[openList[u]] >= Fcost[openList[2*u]]) 
							v = 2*u;
						if (Fcost[openList[v]] >= Fcost[openList[2*u+1]]) 
							v = 2*u+1;		
					} else {
						if (2*u <= numberOfOpenListItems) {
							if (Fcost[openList[u]] >= Fcost[openList[2*u]]) 
								v = 2*u;
						}
					}
	
					if (u != v) {
						temp = openList[u];
						openList[u] = openList[v];
						openList[v] = temp;			
					} else break;
				} while (true);
	
				for (b = parentYval-1; b <= parentYval+1; b++) {
					for (a = parentXval-1; a <= parentXval+1; a++) {
						if (a != -1 && b != -1 && a != mapWidth && b != mapHeight) {
											
							if (whichList[a,b] != onClosedList) { 
								if (isWay(zones[a,b])) { 
										
									corner = walkable;	
									if (a == parentXval-1) {
										if (b == parentYval-1) {
											if (!isWay(zones[parentXval-1,parentYval]) || !isWay(zones[parentXval,parentYval-1]))
												corner = unwalkable;
										} else if (b == parentYval+1) {
											if (!isWay(zones[parentXval,parentYval+1]) || !isWay(zones[parentXval-1,parentYval])) 
												corner = unwalkable; 
										}
									} else if (a == parentXval+1) {
										if (b == parentYval-1) {
											if (!isWay(zones[parentXval,parentYval-1]) || !isWay(zones[parentXval+1,parentYval])) 
												corner = unwalkable;
										} else if (b == parentYval+1) {
											if (!isWay(zones[parentXval+1,parentYval]) || !isWay(zones[parentXval,parentYval+1]))
												corner = unwalkable; 
										}
									}	
									if (corner == walkable) {
										if (whichList[a,b] != onOpenList) {	
	
											newOpenListItemID = newOpenListItemID + 1;
											m = numberOfOpenListItems+1;
											openList[m] = newOpenListItemID;
											openX[newOpenListItemID] = a;
											openY[newOpenListItemID] = b;
												
											if (Math.Abs(a-parentXval) == 1 && Math.Abs(b-parentYval) == 1)
												addedGCost = 18;
											else	
												addedGCost = 10;
											Gcost[a,b] = Gcost[parentXval,parentYval] + addedGCost;
											
											Hcost[openList[m]] = 10*(Math.Abs(a - targetX) + Math.Abs(b - targetY));
											Fcost[openList[m]] = Gcost[a,b] + Hcost[openList[m]];
											parentX[a,b] = parentXval ; parentY[a,b] = parentYval;	
	
											while (m != 1) {
												if (Fcost[openList[m]] <= Fcost[openList[m/2]]) {
													temp = openList[m/2];
													openList[m/2] = openList[m];
													openList[m] = temp;
													m = m/2;
												} else break;
											}
											numberOfOpenListItems = numberOfOpenListItems+1;
	
											whichList[a,b] = onOpenList;
										} else {
											if (Math.Abs(a-parentXval) == 1 && Math.Abs(b-parentYval) == 1)
												addedGCost = 18;	
											else	
												addedGCost = 10;			
											tempGcost = Gcost[parentXval,parentYval] + addedGCost;
			
											if (tempGcost < Gcost[a,b]) {
												parentX[a,b] = parentXval;
												parentY[a,b] = parentYval;
												Gcost[a,b] = tempGcost;		
	
												for (x = 1; x <= numberOfOpenListItems; x++) {
													if (openX[openList[x]] == a && openY[openList[x]] == b) {
														Fcost[openList[x]] = Gcost[a,b] + Hcost[openList[x]];
														m = x;
														while (m != 1) {
															if (Fcost[openList[m]] < Fcost[openList[m/2]]) {
																temp = openList[m/2];
																openList[m/2] = openList[m];
																openList[m] = temp;
																m = m/2;
															} else break;
														} break;
													}
												}
											}
	
										}
									}
								}
							}
						}
					}
				}

			} else {
				path = nonexistent; break;
			}  
	
			if (whichList[targetX,targetY] == onOpenList) {
				path = found; break;
			}
	
		} while (true);
		
		MuCoord[] coords = new MuCoord[mapWidth*mapHeight];
		int j = 0;
		int tempx = 0;
		if (path == found) {
			
			pathX = targetX; pathY = targetY;
			coords[j] = new MuCoord(){ x = pathX, y = pathY }; j++;
			do {
				tempx = parentX[pathX,pathY];
				pathY = parentY[pathX,pathY];
				pathX = tempx;
				
				if (pathX != startX || pathY != startY) {
					coords[j] = new MuCoord(){ x = pathX, y = pathY };
					j++;
				}
			} while (pathX != startX || pathY != startY);
			
			
			
		}
		
		Array.Resize(ref coords, j);
		Array.Reverse(coords);
		
		return coords;
	}
	
	
	
	public static bool isWay(byte zone) {
		return Util.Map.isWalkZone(zone);
	}
	
	
}
