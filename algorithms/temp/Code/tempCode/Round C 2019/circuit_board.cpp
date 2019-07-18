#include <iostream>
#include <string.h>
#include <vector>
#include <stack>

using namespace std;
#define MAX_INT 310
int maze[MAX_INT][MAX_INT];
int cord[MAX_INT][MAX_INT];

int getMaxArea(vector<int> vec)
{
    // Create an empty stack. The stack holds indexes
    // of hist[] array. The bars stored in stack are
    // always in increasing order of their heights.
    stack<int> s;
    int n = vec.size();

    int max_area = 0; // Initalize max area
    int tp;  // To store top of stack
    int area_with_top; // To store area with top bar
                       // as the smallest bar

    // Run through all bars of given histogram
    int i = 0;
    while (i < n)
    {
        // If this bar is higher than the bar on top
        // stack, push it to stack
        if (s.empty() || vec[s.top()] <= vec[i])
            s.push(i++);

        // If this bar is lower than top of stack,
        // then calculate area of rectangle with stack
        // top as the smallest (or minimum height) bar.
        // 'i' is 'right index' for the top and element
        // before top in stack is 'left index'
        else
        {
            tp = s.top();  // store the top index
            s.pop();  // pop the top

            // Calculate the area with vec[tp] stack
            // as smallest bar
            area_with_top = vec[tp] * (s.empty() ? i :
                                   i - s.top() - 1);

            // update max area, if needed
            if (max_area < area_with_top)
                max_area = area_with_top;
        }
    }

    // Now pop the remaining bars from stack and calculate
    // area with every popped bar as the smallest bar
    while (s.empty() == false)
    {
        tp = s.top();
        s.pop();
        area_with_top = vec[tp] * (s.empty() ? i :
                                i - s.top() - 1);

        if (max_area < area_with_top)
            max_area = area_with_top;
    }

    return max_area;
}

int main()
{
    int t;
    int r, c, k;
    int ans, max_v, min_v;
    vector<int> vec;
    cin>>t;
    for(int i=0; i<t; i++) {
        cin>>r>>c>>k;
        memset(maze, 0, sizeof(maze));
        ans = r;
        for(int rr = 0; rr<r; rr++) {
            for (int cc =0; cc<c; cc++) {
                cin>>maze[rr][cc];
            }
        }
        for(int rr = 0; rr<r; rr++) {
            for (int cc =0; cc<c; cc++) {
                max_v = maze[rr][cc];
                min_v = maze[rr][cc];
                cord[rr][cc] = cc;
                for(int cnt = cc+1; cnt <c; cnt ++) {
                    if(maze[rr][cnt] > max_v) max_v = maze[rr][cnt];
                    if(maze[rr][cnt] < min_v) min_v = maze[rr][cnt];
                    if(max_v - min_v <= k) {cord[rr][cc] = cnt;}
                    else break;
                }
            }
        }

//        for(int j=0; j<r; j++)
//        {
//            for(int k=0; k<c; k++)
//            {
//                cout<<cord[j][k];
//            }
//            cout<<endl;
//        }
//        cout<<"-------"<<endl;
        for(int cc = 0; cc<c; cc++) {
            vec.clear();
            for(int rr=0; rr<r; rr++) {
                vec.push_back(cord[rr][cc] - cc +1);
            }
            ans = max(ans, getMaxArea(vec));
        }
        cout<<"Case #"<<i+1<<": "<<ans<<endl;
    }
    return 0;
}
