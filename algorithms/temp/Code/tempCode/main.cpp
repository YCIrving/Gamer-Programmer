/*
    class Solution {
    public:
        vector<int> relativeSortArray(vector<int>& arr1, vector<int>& arr2) {

            vector<int> ret;
            map<int, int> intMap;
            for(int i=0; i<arr1.size(); i++)
            {
                int cur = arr1[i];
                if(intMap.find(cur)==intMap.end())
                {
                    intMap[cur] = 1;
                }
                else
                {
                    intMap[cur] ++;
                }
            }
            for(int i=0; i<arr2.size(); i++)
            {
                int count = intMap[arr2[i]];
                while(count>0)
                {
                    ret.push_back(arr2[i]);
                    count--;
                }
                intMap.erase(arr2[i]);
            }

            for (map<int, int>::iterator it=intMap.begin(); it!=intMap.end(); ++it)
            {
                int num = it->first, count = it->second;
                while(count>0)
                {
                    ret.push_back(num);
                    count --;
                }
            }
            return ret;
        }
    };*/
