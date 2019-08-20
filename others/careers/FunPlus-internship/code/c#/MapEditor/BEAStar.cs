using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///-----------------------------------------------------------------------------------------
///   Namespace:      BE
///   Class:          BEAStar
///   Description:    implement of A star path finding algorithm
///   Usage :		  Init(32,32,1.0f);
///                   SetTileMode(10,10,0);
///                   PathFind(tileStart, tileEnd, tileExcepts);
///   Author:         BraveElephant inc.                    
///   Version: 		  v1.0 (2015-11-15)
///-----------------------------------------------------------------------------------------
namespace BE {

	// class for represent each tile
	//[System.Serializable]
	//[ExecuteInEditMode]
	public class AStarTile {
		public int 			x;			// tile index of x coordinate
		public int 			y;			// tils index of z(y) coordinate
		public int 			type;		// tile is blank or filed. if 0 the tile is blank
		public int 			type2;		// 
		public AStarTile 	parent;		// previous tile (when path finding completed, each tiles are connected with this variable)
		public int 			open;		// used while path find algorithm
		public int 			g;			// used while path find algorithm
		public int 			h;			// used while path find algorithm
		public int 			f;			// used while path find algorithm
		public GameObject 	goTile = null;	// if tile has gameobject, set this value

		// initialize tile with x, y cordination index
		public AStarTile(int posx,int posy) {
			x = posx;
			y = posy;
			Reset(true);
		}

		// reset variables 
		public void Reset(bool SetBlank) {
			parent = null;
			open = -1;
			g = 0;
			h = -1;
			f = 0;

			// id set blank set tyle variable to zero
			if(SetBlank)
				SetType(0);
		}

		public void SetType(int _type) {
			//Debug.Log ("AStarTile::SetType "+type.ToString ()+" "+x.ToString()+","+y.ToString());
			type = _type;
			type2 = _type;
			if(goTile == null) 
				return;
			// set material to gameobject by type value
			Material mat = null;
			if(type == 0) 	mat = (Material)Resources.Load ("Materials/Tile", typeof(Material));
			else 			mat = (Material)Resources.Load ("Materials/TileOff", typeof(Material));
			goTile.GetComponent<Renderer>().material = mat;
		}
	}

	// class for A star path finding algorithm
	//[System.Serializable]
	//[ExecuteInEditMode]
	public class BEAStar : MonoBehaviour {

		public	int 			width = 0;			// map width (x coordinate)
		public	int 			height = 0;			// map height(z(y) coordinate)
		public	float 			tilesize = 1.0f;	// actual tile size 
		public 	AStarTile [,] 	tiles = null;		// 2 dimention tile array
		private int [] 			dirX = null;		// movable surrounding tile index
		private int [] 			dirY = null;		// movable surrounding tile index
		private int [] 			Cost = null;		// cost of movable surrounding tiles

		private AStarTile 		tileStart = null;		// start tile pos
		private AStarTile 		tileEnd = null;			// end(destination) tile pos
		private AStarTile 		tileInCheck = null;
		private List<AStarTile> tileExcept=new List<AStarTile>();	// tiles excepted from path finding
		public  List<Vector3> 	Path=new List<Vector3>();			// real position array of finded path

		public  bool PathFindSuccess = false; 		
		public  bool PathFindCompleted = false; 
		public  int  PathCount = 0; 
		public  int  CheckStep = 0; 

		// initialize map
		public void Init(int w, int h, float _tilesize) {

			// set surrounding tile index and costr
			dirX = new int[8] { -1, 0, 1, 1, 1, 0,-1,-1 };
			dirY = new int[8] { -1,-1,-1, 0, 1, 1, 1, 0 };
			Cost = new int[8] { 14,10,14,10,14,10,14,10 };

			width = w;
			height = h;
			tilesize = _tilesize;

			//create each tile 
			tiles = new AStarTile[w,h];
			for(int y=0 ; y < height ; ++y) {
				for(int x=0 ; x < width ; ++x) {
					tiles[x,y] = new AStarTile(x,y);
				}
			}
		}

		// find tile with given type
		public AStarTile GetTile(int type) {
			for(int y=0 ; y < height ; ++y) {
				for(int x=0 ; x < width ; ++x) {
					if(tiles[x,y].type == type) {
						return tiles[x,y];
					}
				}
			}

			return null;
		}

		// get tile with actual position
		public AStarTile GetTile(Vector3 vPos) {
			int x = (int)((vPos.x + (float)(width-1)*0.5f*tilesize)/tilesize);
			int z = (int)((vPos.z + (float)(height-1)*0.5f*tilesize)/tilesize);
			x = Mathf.Clamp(x, 0, width-1);
			z = Mathf.Clamp(z, 0, height-1);
			return tiles[x,z];
		}

		// get actual position with tile x,y index
		public Vector3 GetTilePos(int x, int z) {
			return new Vector3(((float)x - (float)(width-1)*0.5f)*tilesize,0,((float)z - (float)(height-1)*0.5f)*tilesize);
		}

		// reset map data
		// after path finding completed, call this before next pathfinding start
		public void Reset(bool SetBlank) {
			PathFindCompleted = false;
			if(SetBlank) {
				tileStart = null;
				tileEnd = null;
				tileInCheck = null;
			}

			for(int y=0 ; y < height ; ++y) {
				for(int x=0 ; x < width ; ++x) {
					tiles[x,y].Reset(SetBlank);
				}
			}
		}

		// get an empty(blank) tile randomly
		public AStarTile FindEmpty(int ExceptX, int ExceptY) {
			int iCount = 0;
			for(int y=0 ; y < height ; ++y) {
				for(int x=0 ; x < width ; ++x) {
					if((tiles[x,y].type == 0) && (tiles[x,y].type2 == 0) && (x != ExceptX) && (y != ExceptY))
						iCount++;
				}
			}

			if(iCount == 0) return null;
			
			int iOffset = UnityEngine.Random.Range (0,iCount);
			iCount = 0;
			for(int y=0 ; y < height ; ++y) {
				for(int x=0 ; x < width ; ++x) {
					if((tiles[x,y].type == 0) && (tiles[x,y].type2 == 0) && (x != ExceptX) && (y != ExceptY)) {
						if(iOffset == iCount) return tiles[x,y];
						iCount++;
					}
				}
			}

			return null;
		}

		// set tile type value with given x,y index
		public void SetTileMode(int x, int y, int type) {
			AStarTile tile = tiles[x,y];

			//if((type == AStarTileType.Start) && (tileStart != null)) { tileStart.SetType(0); tileStart = null; }
			//if((type == AStarTileType.End  ) && (tileEnd   != null)) { tileEnd.SetType(0);   tileEnd = null; }

			tile.SetType(type);

			//if(type == AStarTileType.Start)	tileStart = tile;
			//if(type == AStarTileType.End  ) 	tileEnd = tile;
		}

		// manhattan method function for approximate distance to destination
		private int ManhattanDistance(AStarTile start, AStarTile end) {
			return (Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y))*10;
		}

		// chevyshev method function for approximate distance to destination
		private int ChebyshevDistance(AStarTile start, AStarTile end) {
			return Mathf.Max(Mathf.Abs(end.x - start.x),Mathf.Abs(end.y - start.y))*10;
		}

		// check surrounding tiles with given tile
		private void Check(AStarTile parent) {
			for(int i=0 ; i < 8 ; ++i) {

				int newX = parent.x + dirX[i];
				int newY = parent.y + dirY[i];
				if((newX < 0) || (width <= newX) || (newY < 0) || (height <= newY)) continue;

				AStarTile tile = tiles[newX,newY];

				if((tile != tileEnd) && ((tile.open == 0) || (tile.type != 0) || (tile.type2 != 0))) continue;

				if(tileExcept != null) {
					bool bExcept = false;
					for(int j=0 ; j < tileExcept.Count ; ++j) {
						if(tileExcept[j] == tile) {
							bExcept = true;
							break;
						}
					}

					if(bExcept) continue;
				}
				//if((newY-1 >= 0) && (tiles[newX,newY-1].eType != AStarTileType.Blank)) continue;

				// set tiles value
				if(tile.open == -1) {
					tile.g = parent.g + Cost[i];
					if(tile.h == -1) tile.h = ManhattanDistance(tile, tileEnd);
					tile.f = tile.g + tile.h;
					tile.open = 1;
					tile.parent = parent;
				}
				else { // (tile.open == 1)
					int gNew = parent.g + Cost[i];
					if(gNew < tile.g) {
						tile.g = gNew;
						tile.f = tile.g + tile.h;
						tile.parent = parent;
					}
				}
			}
		}

		//path finding function
		public bool PathFind(AStarTile start, AStarTile end, List<AStarTile> except) {
			Reset(false);

			Path.Clear();
			PathFindSuccess = false;
			PathFindCompleted = false; 
			PathCount = 0;
			CheckStep = 0;
			tileStart = start;
			tileEnd = end;

			tileInCheck = tileStart;
			tileInCheck.open = 0;
			tileInCheck.g = 0;

			if(except == null) 	tileExcept = new List<AStarTile>();
			else 				tileExcept = except;

			PathCount++;

			while(!PathFindCompleted) {
				PathFindSub();
			}

			return PathFindSuccess;
		}

		// sub functions call recursively while path finding
		public void PathFindSub() {
			CheckStep++;
			Check(tileInCheck);

			AStarTile tileMinF = null;
			for(int y=0 ; y < height ; ++y) {
				for(int x=0 ; x < width ; ++x) {
					if(tiles[x,y].open != 1) continue;
						
					if((tileMinF == null) || (tiles[x,y].f < tileMinF.f))
						tileMinF = tiles[x,y];
				}
			}
				
			if(tileMinF == null) {
				PathFindCompleted = true;
				//Debug.Log ("A Star : Find Path Fail");
				return;
			}

			tileInCheck = tileMinF;
			tileInCheck.open = 0;
			//Debug.Log ("A Star "+PathCount.ToString ()+" - "+tileInCheck.x.ToString()+","+tileInCheck.y.ToString()+",g:"+tileInCheck.g.ToString()+",h:"+tileInCheck.h.ToString()+",f:"+tileInCheck.f.ToString());
			PathCount++;

			//add finded tile to path array
			if(tileInCheck == tileEnd) {
				PathFindCompleted = true;
				PathFindSuccess = true;

				AStarTile tileResult = tileEnd;
				while(tileResult != null) {
					//Path.Insert(0, tileResult.goTile.transform.position);
					Path.Insert(0, GetTilePos(tileResult.x, tileResult.y));
					//Debug.Log ("FinalPath - "+tileResult.x.ToString()+","+tileResult.y.ToString());
					tileResult = tileResult.parent;
				}
			}
		}
	}

}