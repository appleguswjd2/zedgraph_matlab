 function [x,y]= mTcPlot(fileNum)

    dd = dir('C:\Users\wyz\Desktop\jiangting\bin\*.bin');%find the 'bin'file in that directory
    fileNames = {dd.name};%Store the file name 

    binfilelist = cell(numel(fileNames),1); %make empty cell
    binfilelist(:,1) = regexprep(fileNames, '.bin','');%put all the file name in array
    
    i=fileNum;

    f(i) = strcat('C:\Users\wyz\Desktop\jiangting\bin\', fileNames(i));%append the name with directory
    fid = fopen(char(f(i)));
    % fid = fopen('C:\Users\wyz\Desktop\jiangting\bin\1f2and1f2dipolar0.04ac1000rf100022_57_56.bin');
    temp = fread(fid, [2 inf], 'uint16=>double');
    fclose(fid);
    data = complex(temp(1,:), temp(2,:));
    n=1:1:length(data);
    x=n*0.1/50000;
    y=data(n);%Ion Current Intensity
   

% j=1:N;
% plot(j,IC(j));