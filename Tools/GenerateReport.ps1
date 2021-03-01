# generate csv report from train result folder

param(
    [string]$TrainResultFolder,
    [string]$ReportFileName,
    [string]$mlnetVersion
)


$mbConfigs = Get-ChildItem $TrainResultFolder -Recurse | where {$_.Extension -eq ".mbconfig"}
$csvs = @()
foreach ($mbConfig in $mbConfigs)
{
    $json = Get-Content $mbConfig.Fullname | ConvertFrom-Json
    $runHistory = $json.RunHistory
    $metricName = $runHistory.MetricName
    $datasetName = $mbConfig.Basename
    $i = 1
    foreach($trial in $runHistory.Trials)
    {
        Add-Member -InputObject $trial -Name "Version" -Value $mlnetVersion -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "Dataset" -Value $datasetName -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "IterationIndex" -Value $i -MemberType NoteProperty
        $i = $i+1
        $csvs += $trial
    }
}


$csvs | Select-Object -Property TrainerName,Score,RuntimeInSeconds,Version,Dataset,IterationIndex | Export-Csv -Path $ReportFileName -NoTypeInformation 
