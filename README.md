Library for calculating outline from a set of dissolved (GPS) tracks.

WARNING: Library has an issue with breaking paths at intersection. Here is the solution.

Library for calculating outline from a set of (GPS) tracks is provided. Tracks should be dissolved (i.e. split at intersection points). The advantage in comparison to \*hull algorithms is that current solution uses the already existing outline curve information. The main idea behind algorithm is to pick correct curves that lay on the outline. This is performed by picking the initial track and adding to the path curves that have the smallest angle between the path end vector and candidate curve. This way outline is collected from available curves in clockwise direction.

Library is able to deal with multiple disconnected areas in which case speed decreases several times because of the need to mark inner curves. This could be fixed by implementing initial curve diffusion that would build a neighboring curve graph. This would increase speed both of the outline calculation and used curve marking.

Sample app provided for anyone to draw intersectable curves and test algorithm.

Initial problem was formulated here: [GIS StackExchange](http://gis.stackexchange.com/questions/102559/outline-from-multiple-tracks).