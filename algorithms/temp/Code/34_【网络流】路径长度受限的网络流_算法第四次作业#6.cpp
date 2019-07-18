//Onenote 动态规划：路径长度受限的网络流
/*
 * Dinic algo for max flow
 *
 * This implementation assumes that #nodes, #edges, and capacity on each edge <= INT_MAX,
 * which means INT_MAX is the best approximation of INF on edge capacity.
 * The total amount of max flow computed can be up to LLONG_MAX (not defined in this file),
 * but each 'dfs' call in 'dinic' can return <= INT_MAX flow value.
 */
#include <stdio.h>
#include <stdlib.h>
#include <limits.h>
#include <string.h>
#include <assert.h>
#include <queue>
#include <vector>
#include <algorithm>
#include <iostream>

#define N (100+2)
#define M (10000) //少了可能超时

typedef long long LL;

using namespace std;

struct edge {
  int v, cap, next;
};
edge e[M];

int head[N], level[N], cur[N];
int num_of_edges;
int dist[N][N];
bool is_legal[N][N];

/*
 * When there are multiple test sets, you need to re-initialize before each
 */
void dinic_init(void) {
  num_of_edges = 0;
  memset(head, -1, sizeof(head));
  return;
}

int add_edge(int u, int v, int c1, int c2) {
  int& i=num_of_edges;

  assert(c1>=0 && c2>=0 && c1+c2>=0); // check for possibility of overflow
  e[i].v = v;
  e[i].cap = c1;
  e[i].next = head[u];
  head[u] = i++;

  e[i].v = u;
  e[i].cap = c2;
  e[i].next = head[v];
  head[v] = i++;
  return i;
}

void print_graph(int n) {
  for (int u=0; u<n; u++) {
    printf("%d: ", u);
    for (int i=head[u]; i>=0; i=e[i].next) {
      printf("%d(%d)", e[i].v, e[i].cap);
    }
    printf("\n");
  }
  return;
}

/*
 * Find all augmentation paths in the current level graph
 * This is the recursive version
 */
int dfs(int u, int t, int bn) {
  if (u == t) return bn;
  int left = bn;
  for (int &i=cur[u]; i>=0; i=e[i].next) {
    int v = e[i].v;
    int c = e[i].cap;
    if (c > 0 && level[u]+1 == level[v]) {
      int flow = dfs(v, t, min(left, c));
      if (flow > 0) {
        e[i].cap -= flow;
        e[i^1].cap += flow;
        cur[u] = i;
        left -= flow;
        if (!left) break;
      }
    }
  }
  if (left > 0) level[u] = 0;
  return bn - left;
}

bool bfs(int s, int t) {
  memset(level, 0, sizeof(level));
  level[s] = 1;
  queue<int> q;
  q.push(s);
  while (!q.empty()) {
    int u = q.front();
    q.pop();
    if (u == t) return true;
    for (int i=head[u]; i>=0; i=e[i].next) {
      int v = e[i].v;
      if (!level[v] && e[i].cap > 0) {
        level[v] = level[u]+1;
        q.push(v);
      }
    }
  }
  return false;
}

LL dinic(int s, int t) {
  LL max_flow = 0;

  while (bfs(s, t)) {
    memcpy(cur, head, sizeof(head));
    max_flow += dfs(s, t, INT_MAX);
  }
  return max_flow;
}

int upstream(int s, int n) {
  int cnt = 0;
  vector<bool> visited(n);
  queue<int> q;
  visited[s] = true;
  q.push(s);
  while (!q.empty()) {
    int u = q.front();
    q.pop();
    for (int i=head[u]; i>=0; i=e[i].next) {
      int v = e[i].v;
      if (e[i].cap > 0 && !visited[v]) {
        visited[v] = true;
        q.push(v);
        cnt++;
      }
    }
  }
  return cnt; // excluding s
}
int trans1(int i)
{
    return 2*i-1;
}
int trans2(int i)
{
    return 2*i-2;
}
int main() {
  int m, n, s, t, k;
  int u,v;
  vector < pair<int, int> > vec;

  while(1)
  {
      scanf("%d %d %d", &n, &m, &k);
      if(n==0 && m==0 && k==0)
      {
          return 0;
      }
      dinic_init();
      vec.clear();
      for(int i=1;i<=n; i++)
      {
          for(int j=1; j<=n; j++)
          {
              if(i == j)
              {
                  dist[i][j]=0;
                  is_legal[i][j]=true;
              }
              else
              {
                  dist[i][j]=INT_MAX;
                  is_legal[i][j]=false;
              }
          }
      }
      for(int i=0; i<m; i++)
      {
          scanf("%d %d", &u, &v);
          vec.push_back(make_pair(u, v));
          dist[u][v]=1;
          is_legal[u][v]=true;
      }
      // floyd
      for(int q=1; q<=n; q++)
      {
          for(int i=1; i<=n; i++)
          {
              for(int j=1; j<=n; j++)
              {
                  if(is_legal[i][q] && is_legal[q][j] && dist[i][q]+dist[q][j]<dist[i][j])
                  {
                      dist[i][j]=dist[i][q]+dist[q][j];
                      is_legal[i][j]=true;
                  }
              }
          }
      }

      for(int i=2; i<2*n-2; i+=2)
      {
          add_edge(i, i+1, 1, 0);
      }
      for(int i=0; i<m; i++)
      {
          u = vec[i].first;
          v = vec[i].second;
          if(dist[1][u]+dist[u][v]+dist[v][n]<=k)
          {
              add_edge(trans1(u), trans2(v), 1, 0);
          }
      }
      s = 1, t = 2*n-2;

//      print_graph(t+1);
      int flow = dinic(s, t);
      printf("%d\n", flow);
  }
}
