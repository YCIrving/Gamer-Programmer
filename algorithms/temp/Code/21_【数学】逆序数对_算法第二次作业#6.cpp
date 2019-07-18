// OneNote 数学->逆序数对

#include <iostream>
using namespace std;
void printInversePair(int n, int* arr);
int mergeSort(int* arr, int l, int r);
int merge(int* arr, int l, int m, int r);
void printInversePair(int n, int* arr)
{
    // n=4, l=0, m=1, r=3
    cout<<mergeSort(arr, 0, n-1)<<endl;
}
int mergeSort(int* arr, int l, int r)
{
    int ans=0, m=(r+l)/2;
    if(r>l)
    {
        ans = mergeSort(arr, l, m)+ mergeSort(arr, m+1, r);
        ans += merge(arr, l, m, r);
    }
    return ans;
}
int merge(int* arr, int l, int m, int r)
{
    int ans=0;
    int temp[r-l+1]={0};
    int i=l, j=m+1, cnt=0;
    while(i<=m && j<= r)
    {
        if(arr[i]<=arr[j])
        {
            temp[cnt++]=arr[i++];
        }
        else
        {
            temp[cnt++]=arr[j++];
            ans+=(m-i+1);
        }
    }
    while(i<=m)
    {
        temp[cnt++]=arr[i++];
    }
    while(j<=r)
    {
        temp[cnt++]=arr[j++];
    }
    for(cnt=l;cnt<=r;cnt++)
    {
        arr[cnt]=temp[cnt-l];
    }
    return ans;
}
int main()
{
    int n=0;
    int arr[20001]={0};
    ios::sync_with_stdio(false);
    cin.tie(0);
    while(cin>>n)
    {
        if(n==0)
        {
            break;
        }
        for(int i=0;i<n;i++)
        {
            cin>>arr[i];
        }
        printInversePair(n, arr);
//        for(int i=0;i<n;i++)
//        {
//            cout<<arr[i]<<' ';
//        }
//        cout<<endl;
    }
}
