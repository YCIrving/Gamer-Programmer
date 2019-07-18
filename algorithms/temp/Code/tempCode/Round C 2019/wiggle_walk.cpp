#include <iostream>
#include <string>
#include <set>

using namespace std;


int main()
{
    int t;
    int n,r,c,start_r,start_c;
    int current_r, current_c;
    string s;
    set< pair<int, int> > my_set;
    cin>>t;
    for(int i=0; i<t; i++) {
        cin>>n>>r>>c>>start_r>>start_c;
        cin>>s;
        my_set.clear();
        current_r = start_r;
        current_c = start_c;
        my_set.insert(make_pair(current_r, current_c));
        for(int j=0; j<n; j++) {
            char c = s[j];
            if(c == 'N') {
                while(1) {
                    current_r--;
                    if(my_set.find(make_pair(current_r, current_c)) == my_set.end() ) {
                            my_set.insert(make_pair(current_r, current_c)); break;
                    }
                }
            } else if(c == 'S') {
                while(1) {
                    current_r++;
                    if(my_set.find(make_pair(current_r, current_c)) == my_set.end() ) {
                            my_set.insert(make_pair(current_r, current_c)); break;
                    }
                }
            } else if(c == 'W') {
                while(1) {
                    current_c--;
                    if(my_set.find(make_pair(current_r, current_c)) == my_set.end() ) {
                            my_set.insert(make_pair(current_r, current_c)); break;
                    }
                }
            } else {
                while(1) {
                    current_c++;
                    if(my_set.find(make_pair(current_r, current_c)) == my_set.end() ) {
                            my_set.insert(make_pair(current_r, current_c)); break;
                    }
                }
            }
        }
        cout<<"Case #"<<i+1<<": "<<current_r<<' '<<current_c<<endl;
    }
    return 0;
}
