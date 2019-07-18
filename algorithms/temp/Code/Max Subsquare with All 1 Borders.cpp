 #include <iostream>
 #include <math.h>
 #define MAXN 10000
 using namespace std;
 int arr[MAXN][MAXN];
 int dpx[MAXN][MAXN], dpy[MAXN][MAXN], dp[MAXN][MAXN];
 int main()
 {
     int n;
     int ans = 1;
     cin>>n;
     for(int i=0; i<n; i++) {
         for(int j=0; j<n; j++) {
             cin>>arr[i][j];
         }
     }

     for(int i=0; i<n; i++) dpx[i][n-1] = arr[i][n-1];
     for(int i=0; i<n; i++) {
         for(int j=n-2; j>=0; j--) {
             if (arr[i][j] == 0) dpx[i][j] = 0;
             else dpx[i][j] = dpx[i][j+1] + 1;
         }
     }

     for(int i=0; i<n; i++) dpy[n-1][i] = arr[n-1][i];
     for(int i=n-2; i>=0; i--) {
         for(int j=0; j<n; j++) {
             if (arr[i][j] == 0) dpy[i][j] = 0;
             else dpy[i][j] = dpy[i+1][j] + 1;
         }
     }


     for(int i=0; i<n; i++) {
         for(int j=0; j<n; j++) {
             int length = min(dpx[i][j], dpy[i][j]);
             for(int k = length; k>1; k--) {
                 if(dpx[i+k-1][j]>=k && dpy[i][j+k-1] >= k) {
                     ans = max(ans, k);
                     break;
                 }
             }
         }
     }
     cout<<ans<<endl;
     return 0;
 }
