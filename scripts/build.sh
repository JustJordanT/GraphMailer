#!/bin/bash
set -e

current_directory="$PWD"

cd GraphMailer.Email

dotnet restore

dotnet build
