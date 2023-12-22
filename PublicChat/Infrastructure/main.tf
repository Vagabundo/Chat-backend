# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.65"
    }
  }

  required_version = ">= 0.14.9"
}

terraform {
  backend "azurerm" {
    resource_group_name  = "tf_rg_blobstore"
    storage_account_name = "tfstorageaccvaga"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}

# terraform {
#   backend "remote" {
#     organization = "Vagacorp"

#     workspaces {
#       name = "web-chat"
#     }
#   }
# }

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location

  tags = {
    Environment = "Web chat"
    Team        = "VagaDevOps"
  }
}

# resource "azurerm_app_service_plan" "freeplan" {
#   name                = "${var.resource_group_name}-plan"
#   location            = azurerm_resource_group.rg.location
#   resource_group_name = azurerm_resource_group.rg.name

#   sku {
#     tier = "Free"
#     size = "F1"
#   }
# }

resource "azurerm_container_group" "webchat_containergroup" {
  name                = "${var.resource_group_name}-containergroup"
  location            = var.location
  resource_group_name = var.resource_group_name

  ip_address_type = "public"
  dns_name_label  = "vagabundo-${var.project_name}"
  os_type         = "Linux"

  container {
    name   = var.project_name
    image  = "vagabundocker/${var.project_name}:${var.imagebuild}"
    cpu    = "1"
    memory = "1"

    ports {
      port     = 80
      protocol = "TCP"
    }
  }

  tags = {
    Environment = "Web chat"
  }

  # esto no sería necesario si mas arriba usaramos azurerm_resource_group.rg.location,
  # pero lo dejo como ejemplo de uso de depends_on
  depends_on = [azurerm_resource_group.rg]
}

# resource "azurerm_app_service_plan" "linuxfreeplan" {
#   name                = "${var.resource_group_name}-linuxplan"
#   location            = azurerm_resource_group.rg.location
#   resource_group_name = azurerm_resource_group.rg.name
#   kind = "Linux"
#   reserved = true

#   sku {
#     tier = "Free"
#     size = "F1"
#   }
# }

# resource "azurerm_app_service" "vagachatappservice" {
#   name                = "${var.resource_group_name}-appservice"
#   location            = azurerm_resource_group.rg.location
#   resource_group_name = azurerm_resource_group.rg.name
#   app_service_plan_id = azurerm_app_service_plan.freeplan.id

#   site_config {
#     dotnet_framework_version = "v6.0"
#     # linux_fx_version          = "DOTNETCORE|3.1"
#     use_32_bit_worker_process = true
#   }

#   tags = {
#     Environment = "Web chat"
#   }
# }