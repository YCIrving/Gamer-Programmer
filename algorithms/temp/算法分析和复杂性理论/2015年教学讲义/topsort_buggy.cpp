/*
 * Author: Zhen Xiao
 * Topological sort based on Algorithm Design
 * The following code is incomplete and is inconsistent with the node index in the input
 * You need to fix them in order to pass Homework 2.
 */
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <vector>
#include <queue>

using namespace std;

vector<int> topsort(const vector<vector<int> > &lnklst) {
  int n = lnklst.size();
  vector<int> d(n);
  for (int i=0; i<n; i++) {
    for (int j=0; j<lnklst[i].size(); j++) {
      int v = lnklst[i][j];
      d[v]++;
    }
  }
  queue<int> q;
  for (int i=0; i<n; i++) {
    if (!d[i]) { q.push(i); }
  }

  vector<int> result;
  while (!q.empty()) {
    int u = q.front();
    q.pop();
    result.push_back(u);
    for (int j=0; j<lnklst[u].size(); j++) {
      int v = lnklst[u][j];
      d[v]--;
      if (!d[v]) { q.push(v); }
    }
  }
  return result;
}

int main() {
  return 0;
}
