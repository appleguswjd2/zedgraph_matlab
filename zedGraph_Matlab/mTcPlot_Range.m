function [x,averageIC]= mTcPlot_Range(selectedfileNumber_start,selectedfileNumber_end,xLength)
   
%  selectedfileNumber_start=1;
%  selectedfileNumber_end=17;
%  xLength=25000;
    dd = dir('C:\Users\wyz\Desktop\jiangting\bin\*.bin');%find the 'bin'file in that directory
    fileNames = {dd.name};%Store the file name 

    binfilelist = cell(numel(fileNames),1); %make empty cell
    binfilelist(:,1) = regexprep(fileNames, '.bin','');%put all the file name in array
 
    fnstart=selectedfileNumber_start;%10
    fnend=selectedfileNumber_end;%13
    N=fnend-fnstart+1;%Number of selected files

    IC=zeros(N,xLength);

for i=fnstart:1:fnend
    f(i) = strcat('C:\Users\wyz\Desktop\jiangting\bin\', fileNames(i));%append the name with directory
    fid = fopen(char(f(i)));
    % fid = fopen('C:\Users\wyz\Desktop\jiangting\bin\1f2and1f2dipolar0.04ac1000rf100022_57_56.bin');
    temp = fread(fid, [2 inf], 'uint16=>double');
    fclose(fid);
    data = complex(temp(1,:), temp(2,:));
    n=1:1:length(data);
    x=n*0.1/50000;
%   y=data(n);%Ion Current Intensity
   fileOrder=int32(i-fnstart+1);
    for j=1:1:xLength
         IC(fileOrder,j)=data(j);
    end
end
% plot(n,IC(1,:))
averageIC=mean(IC);

%%%%%%%average%%%%%%%%%%
