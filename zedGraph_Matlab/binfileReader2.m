function [IC,N]= binfileReader2

dd = dir('C:\Users\wyz\Desktop\jiangting\bin\*.bin');%find the 'bin'file in that directory
fileNames = {dd.name};%Store the file name 

binfilelist = cell(numel(fileNames),1); %make empty cell
binfilelist(:,1) = regexprep(fileNames, '.bin','');%put all the file name in array
N=length(fileNames);

for i=1:N
    f(i) = strcat('C:\Users\wyz\Desktop\jiangting\bin\', fileNames(i));%append the name with directory
    fid = fopen(char(f(i)));
    % fid = fopen('C:\Users\wyz\Desktop\jiangting\bin\1f2and1f2dipolar0.04ac1000rf100022_57_56.bin');
    temp = fread(fid, [2 inf], 'uint16=>double');
    fclose(fid);
    data = complex(temp(1,:), temp(2,:));
    x=1:1:length(data);
    y=data(x);%Ion Current Intensity
    yMax= max(y);%Max Ion Current
    IC(:,i)=yMax;%Array of max Intensity from each bin file from the folder
%     xValue_yMax = find(y == max(y), 1, 'first');%Xvalue of maxValue

end
% j=1:N;
% plot(j,IC(j));