# run trial using mlnet.cli

param(
    [string]$AutoMLType,
    [string]$MlNet,
    [string]$MlNetArgs
)

${env:ModelBuilder.AutoMLType}=$AutoMLType
write-host $MlNet $MlNetArgs
Start-Process $MlNet $MlNetArgs -NoNewWindow -Wait