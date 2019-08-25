//Onenote ÍøÂçÁ÷£ºÄÌÅ£³Ô·¹
/*
 * Dinic algo for max flow POJ1149
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

// remember to modify these two parameters
#define N (500)
#define M (21000)

typedef long long LL;

using namespace std;

/*
 * v stores edge (u, v)'s end note
 */
struct edge {
  int v, cap, next;
};
edge e[M];
struct cow {
    int num_food, num_drink;
    int food[100], drink[100];
} cows[100];
int head[N], level[N], cur[N];
int num_of_edges;

/*
 * When there are multiple test sets, you need to re-initialize before each
 */
void dinic_init(void) {
  num_of_edges = 0;
  memset(head, -1, sizeof(head));
  return;
}

/*
 * Head performs like a link list where head[u] stores index of edges start from u
 * Once a new edge added, we insert it to the begin of link list and modify head[u]
 */

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

/*
 * Only need to know how to use this template
 * m: number of edge
 * n: number of vertex
 * s: start vertex of graph
 * e: end vertex of graph
 */

int main() {
  int n, f, d, s, t;
  int u, v, cap;
  int f_temp, d_temp;
//  int pig[M+1], pre[M+1];
//  bool con[N+1];
//  FILE *fin;

  /*fin = fopen("pigs.dat", "r");
  assert(fin);*/
//  fin = stdin;

    s = 0;
    scanf("%d %d %d",&n, &f, &d);
    for (int i=0; i<n; i++)
    {
        scanf("%d%d", &cows[i].num_food, &cows[i].num_drink);
        for(int j=0;j<cows[i].num_food;j++)
        {
            scanf("%d", &cows[i].food[j]);
        }
        for(int j=0;j<cows[i].num_drink;j++)
        {
            scanf("%d", &cows[i].drink[j]);
        }
    }
    dinic_init();
    for(int i=0;i<f;i++)
    {
        add_edge(0, i+1, 1, 0);
    }
    for(int i=0;i<n;i++)
    {
        for(int j=0;j<cows[i].num_food;j++)
        {
            add_edge(cows[i].food[j], f+i+1, 1, 0);
        }
    }
    for(int i=0;i<n;i++)
    {
        add_edge(f+i+1, f+i+1+n, 1, 0);
    }

    for(int i=0;i<n;i++)
    {
        for(int j=0;j<cows[i].num_drink;j++)
        {
            add_edge(f+i+1+n, cows[i].drink[j]+f+2*n, 1, 0);
        }
    }

    for(int i=0;i<d;i++)
    {
        add_edge(f+2*n+i+1, f+2*n+d+1, 1, 0);
    }
    s = 0, t = f+2*n+d+1;
//    print_graph(t);
    int flow = dinic(s, t);
    printf("%d\n", flow);
    return 0;
}
