using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PriorityQueue<T>
{
    private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();

    public int Count { get { return elements.Count; } }

    public void Enqueue(T item, int priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}

public class TilemapPathfinding : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int startCell;
    public Vector3Int goalCell;

    private List<Vector3Int> path;

    void Start()
    {
        path = FindPath(startCell, goalCell);
        if (path != null)
        {
            // パスが見つかった場合、各位置に移動する処理を実装する
            StartCoroutine(MoveAlongPath());
        }
    }

    List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();

        PriorityQueue<Vector3Int> frontier = new PriorityQueue<Vector3Int>();
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (Vector3Int next in GetNeighbors(current))
            {
                int newCost = costSoFar[current] + GetCost(next);

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        if (cameFrom.ContainsKey(goal))
        {
            // ゴールに到達可能な場合、経路を復元する
            List<Vector3Int> path = new List<Vector3Int>();
            Vector3Int current = goal;

            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Reverse();
            return path;
        }
        else
        {
            // ゴールに到達不可能な場合はnullを返す
            return null;
        }
    }

    IEnumerable<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        // 上下左右の隣接するセルを取得する
        yield return cell + new Vector3Int(0, 1, 0);
        yield return cell + new Vector3Int(0, -1, 0);
        yield return cell + new Vector3Int(1, 0, 0);
        yield return cell + new Vector3Int(-1, 0, 0);
    }

    int GetCost(Vector3Int cell)
    {
        // 各セルの移動コストを設定する
        TileBase tile = tilemap.GetTile(cell);

        // タイルによって異なる移動コストを設定する場合は適宜処理を追加する

        return tile != null ? 1 : int.MaxValue; // 障害物がある場合は非常に高いコストを設定する
    }

    int Heuristic(Vector3Int current, Vector3Int goal)
    {
        // ヒューリスティック関数（推定値）を設定する
        // ここではマンハッタン距離を使用します
        return Mathf.Abs(current.x - goal.x) + Mathf.Abs(current.y - goal.y);
    }

    IEnumerator MoveAlongPath()
    {
        foreach (Vector3Int cell in path)
        {
            Vector3 worldPosition = tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f, 0f); // セルの中心位置に移動する
            // 敵キャラクターの移動処理を実装する（例: transform.position = worldPosition;）

            yield return new WaitForSeconds(0.5f); // 移動ごとに一定の待機時間を設ける
        }
    }
}

