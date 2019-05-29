pragma solidity ^0.4.18;

contract DrugManagement {
    
    struct Company {
        bytes32 name;
        // "bool doesExist" is to check if this Struct exists
        // This is so we can keep track of the companies 
        bool doesExist; 
    }
    
	struct Drug {
		bytes32 name;
	}
	
	struct Transaction {
	    bytes32 uid;
	    uint fromCompanyId;
	    uint toCompanyId;
	    bytes32 drugName;
	    uint amount;
	    string packageId;
	}
    
    // These state variables are used keep track of the number of Companies and Drugs
    // and used to as a way to index them     
    uint totalCompanies;
    uint totalDrugs;
    uint totalTransactions;
    
    // Think of these as a hash table, with the key as a uint and value of 
    // the struct Company. These mappings will be used in the majority
    // of our transactions/calls
    // These mappings will hold all the candidates and Voters respectively
    mapping (uint => Company) companies;
    mapping (uint => Drug) drugs;
	mapping (uint => Transaction) transactions;
	
    function send(bytes32 uid, uint fromCompanyId, uint toCompanyId, bytes32 drugName, uint amount, string packageId) public {
        if (companies[fromCompanyId].doesExist == true && companies[toCompanyId].doesExist == true) {
            uint transactionId = totalTransactions++;
            transactions[transactionId] = Transaction(uid, fromCompanyId, toCompanyId, drugName, amount, packageId);
        }
    }
	
	event AddedCompany(uint companyId);
	function addCompany(bytes32 companyName) public {
		uint companyId = totalCompanies++;
		
		companies[companyId] =  Company(companyName, true);
		
		AddedCompany(companyId);
	}
	
	
    /* * * * * * * * * * * * * * * * * * * * * * * * * * 
    *  Getter Functions, marked by the key word "view" *
    * * * * * * * * * * * * * * * * * * * * * * * * * */
	
	function getTotalCompanies() public view returns(uint) {
        return totalCompanies;
    }
    
    function getCompany(uint companyId) public view returns (uint,bytes32) {
        return (companyId, companies[companyId].name);
    }
    
    function getTotalTransactions() public view returns(uint) {
        return totalTransactions;
    }
    
    function getTransaction(uint transactionId) public view returns(uint, uint, uint, bytes32, uint) {
        return (transactionId, transactions[transactionId].fromCompanyId, transactions[transactionId].toCompanyId,
            transactions[transactionId].drugName, transactions[transactionId].amount);
    }
    
    /* In progress */
    function getCurrentStorageOfCompany(uint companyId) public view returns(bytes32[], uint[]) {
        bytes32[] drugs;
        uint[] drugAmounts;
        
        // Add drugs this company has received
        for(uint i = 0; i < totalTransactions; i++) {
            if (transactions[i].toCompanyId == companyId) {
                drugs.push(transactions[i].drugName);
                drugAmounts.push(transactions[i].amount);
            }
        }
        
        // Then delete drugs that this company has sent away
        
        return (drugs, drugAmounts);
    }
    
}