import requests
import json
import sys
import base64
from cryptography.hazmat.primitives import hashes
from cryptography.hazmat.primitives.kdf.pbkdf2 import PBKDF2HMAC
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.primitives import padding
from generate_basic_auth_credentials import generate_basic_auth_credentials

def generate_key_and_iv():
    """
    Generate AES key and IV using PBKDF2 matching C# AES256Cryptography implementation
    """
    # Password and salt from C# code
    password = "CERS&NicHP@23@ece"
    salt = bytes([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1])  # 13 bytes: 12 zeros + 1
    
    # PBKDF2 with SHA1 (default for Rfc2898DeriveBytes)
    kdf = PBKDF2HMAC(
        algorithm=hashes.SHA1(),
        length=48,  # 32 bytes key + 16 bytes IV
        salt=salt,
        iterations=1000,  # Default for Rfc2898DeriveBytes
    )
    
    # Derive key material
    key_material = kdf.derive(password.encode('utf-8'))
    
    # Split into key and IV
    key = key_material[:32]  # First 32 bytes for AES-256 key
    iv = key_material[32:48]  # Next 16 bytes for IV
    
    return key, iv

def encrypt_aes256_cbc(plaintext, key, iv):
    """
    Encrypt using AES-256-CBC matching C# implementation
    """
    # Convert to UTF-16 (Unicode) like C# Encoding.Unicode
    plaintext_bytes = plaintext.encode('utf-16le')
    
    # Add PKCS7 padding
    padder = padding.PKCS7(128).padder()
    padded_data = padder.update(plaintext_bytes)
    padded_data += padder.finalize()
    
    # Encrypt
    cipher = Cipher(algorithms.AES(key), modes.CBC(iv))
    encryptor = cipher.encryptor()
    encrypted_data = encryptor.update(padded_data) + encryptor.finalize()
    
    # Return base64 encoded
    return base64.b64encode(encrypted_data).decode('utf-8')

def get_bearer_token(base_url="http://10.146.2.8/CERSWebApi"):
    """
    Get Bearer token from GenerateToken endpoint using encrypted Basic Auth
    """
    # Generate encrypted Basic Auth credentials
    creds = generate_basic_auth_credentials()
    
    # Make request to GenerateToken
    url = f"{base_url}/api/GenerateToken"
    headers = {
        "Authorization": creds['authorization_header'],
        "Content-Type": "application/json"
    }
    
    print(f"=== Getting Bearer Token ===")
    print(f"URL: {url}")
    print(f"cURL command:")
    print(f'curl -X GET "{url}" \\')
    print(f'  -H "Authorization: {creds["authorization_header"]}" \\')
    print(f'  -H "Content-Type: application/json"')
    print()
    
    try:
        response = requests.get(url, headers=headers)
        response.raise_for_status()
        
        data = response.json()
        print(f"Response: {json.dumps(data, indent=2)}")
        print()
        
        if data.get('status_code') == 200 and 'TokenID' in data:
            return data['TokenID']
        else:
            print(f"Token generation failed: {data}")
            return None
            
    except requests.exceptions.RequestException as e:
        print(f"Error getting token: {e}")
        return None

def encrypt_mobile_aes256(mobile_number):
    """
    Encrypt mobile number using AES-256 for API calls
    """
    key, iv = generate_key_and_iv()
    return encrypt_aes256_cbc(mobile_number, key, iv)

def check_user_type(mobile_number, token, base_url="http://10.146.2.8/CERSWebApi"):
    """
    Check user type using encrypted mobile number and Bearer token
    """
    # Encrypt mobile number
    encrypted_mobile = encrypt_mobile_aes256(mobile_number)
    
    # Make request to CheckUserType with query parameter
    url = f"{base_url}/api/CheckUserType"
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }
    
    # Add encrypted mobile as query parameter
    params = {
        "MobileNo": encrypted_mobile
    }
    
    print(f"=== CheckUserType for {mobile_number} ===")
    print(f"Encrypted mobile: {encrypted_mobile}")
    print(f"URL: {url}")
    print(f"Query parameter: MobileNo={encrypted_mobile}")
    print(f"cURL command:")
    print(f'curl -X GET "{url}?MobileNo={encrypted_mobile}" \\')
    print(f'  -H "Authorization: Bearer {token}" \\')
    print(f'  -H "Content-Type: application/json"')
    print()
    
    try:
        response = requests.get(url, headers=headers, params=params)
        response.raise_for_status()
        
        data = response.json()
        print(f"Response: {json.dumps(data, indent=2)}")
        print()
        return data
        
    except requests.exceptions.RequestException as e:
        print(f"Error checking user type: {e}")
        print()
        return None

def interactive_mode():
    """
    Interactive mode for manual input
    """
    print("=== CERS API Interactive Mode ===")
    print()
    
    # Get Bearer token first
    print("Step 1: Getting Bearer token...")
    token = get_bearer_token()
    
    if not token:
        print("Failed to get Bearer token. Exiting.")
        return
    
    print(f" Bearer token obtained: {token}")
    print()
    
    while True:
        print("=== Available Options ===")
        print("1. Check User Type")
        print("2. Generate encrypted mobile number")
        print("3. Show current Bearer token")
        print("4. Get new Bearer token")
        print("5. Exit")
        print()
        
        choice = input("Enter your choice (1-5): ").strip()
        
        if choice == "1":
            mobile = input("Enter mobile number: ").strip()
            if mobile:
                check_user_type(mobile, token)
            else:
                print("Invalid mobile number")
                
        elif choice == "2":
            mobile = input("Enter mobile number to encrypt: ").strip()
            if mobile:
                encrypted = encrypt_mobile_aes256(mobile)
                print(f"Mobile: {mobile}")
                print(f"Encrypted: {encrypted}")
                print()
                
        elif choice == "3":
            print(f"Current Bearer token: {token}")
            print()
            
        elif choice == "4":
            print("Getting new Bearer token...")
            new_token = get_bearer_token()
            if new_token:
                token = new_token
                print(f" New Bearer token: {token}")
            print()
            
        elif choice == "5":
            print("Exiting...")
            break
            
        else:
            print("Invalid choice. Please enter 1-5.")
        
        print("-" * 50)

def test_predefined_mobiles(token):
    """
    Test with predefined mobile numbers
    """
    test_mobiles = [
        "9876543210",
        "9999999999",  # Mock candidate
        "8888888888",  # Mock agent
        "7777777777"   # Mock observer
    ]
    
    print("=== Testing Predefined Mobile Numbers ===")
    for mobile in test_mobiles:
        check_user_type(mobile, token)

def main():
    """
    Main function with command line argument support
    """
    if len(sys.argv) > 1:
        if sys.argv[1] == "interactive" or sys.argv[1] == "-i":
            interactive_mode()
            return
        elif sys.argv[1] == "mobile" or sys.argv[1] == "-m":
            if len(sys.argv) > 2:
                mobile = sys.argv[2]
                print(f"=== Testing Mobile: {mobile} ===")
                token = get_bearer_token()
                if token:
                    check_user_type(mobile, token)
                return
            else:
                print("Usage: python test_api_with_token.py mobile <mobile_number>")
                return
        elif sys.argv[1] == "encrypt" or sys.argv[1] == "-e":
            if len(sys.argv) > 2:
                mobile = sys.argv[2]
                encrypted = encrypt_mobile_aes256(mobile)
                print(f"Mobile: {mobile}")
                print(f"Encrypted: {encrypted}")
                return
            else:
                print("Usage: python test_api_with_token.py encrypt <mobile_number>")
                return
        elif sys.argv[1] == "token" or sys.argv[1] == "-t":
            token = get_bearer_token()
            if token:
                print(f"Bearer Token: {token}")
            return
    
    # Default behavior - run predefined tests
    print("=== CERS API Testing with Bearer Token ===")
    print()
    
    # Get Bearer token
    token = get_bearer_token()
    
    if not token:
        print("Failed to get Bearer token. Exiting.")
        return
    
    # Test predefined mobiles
    test_predefined_mobiles(token)
    
    print("=== Usage Examples ===")
    print("python test_api_with_token.py interactive     # Interactive mode")
    print("python test_api_with_token.py mobile 9876543210  # Test specific mobile")
    print("python test_api_with_token.py encrypt 9876543210 # Encrypt mobile only")
    print("python test_api_with_token.py token           # Get Bearer token only")

if __name__ == "__main__":
    main()
