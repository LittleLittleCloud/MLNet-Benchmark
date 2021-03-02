# generate csv report from train result folder

param(
    [string]$TrainResultFolder,
    [string]$ReportFileName,
    [string]$AutoMLType,
    [string]$MlnetVersion
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
        Add-Member -InputObject $trial -Name "AutoMLType" -Value $AutoMLType -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "Version" -Value $MlnetVersion -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "Dataset" -Value $datasetName -MemberType NoteProperty
        Add-Member -InputObject $trial -Name "IterationIndex" -Value $i -MemberType NoteProperty
        $i = $i+1
        $csvs += $trial
    }
}


$csvs | Select-Object -Property * | Export-Csv -Path $ReportFileName -NoTypeInformation 
