/*
 * Author: Zhen Xiao
 * Topological sort based on DFS
 * The following code is incomplete and is inconsistent with the node index in the input
 * You need to fix them in order to pass Homework 2.
 */
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <algorithm>
#include <vector>

using namespace std;

vector<vector<int> > lnklst;
vector<bool> visited;
vector<int> result;

void dfs(int r) {
  if (visited[r]) return;
  for (int i=0; i<lnklst[r].size(); i++) {
    dfs(lnklst[r][i]);
  }
  visited[r] = true;
  result.push_back(r);
  return;
}

vector<int> topsort(const vector<vector<int> > &lnklst) {
  int n = lnklst.size();
  visited.clear();
  visited.resize(n);
  result.clear();
  for (int i=0; i<n; i++) {
    if (!visited[i]) dfs(i);
  }
  reverse(result.begin(), result.end());
  return result;
}

int main() {
  return 0;
}
