from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.primitives import padding
import base64
import urllib.parse

def generate_aes128_key_iv():
    """
    Generate AES-128 key and IV matching CERSWebApi AESCryptography implementation
    """
    # Key from CERSWebApi
    key_string = "CERS&NicHP@23@ece"
    
    # Take first 16 bytes for 128-bit key (matches C# Array.Copy logic)
    key_bytes = key_string.encode('utf-8')[:16]
    
    # Use same key as IV (matches C# implementation)
    iv_bytes = key_bytes
    
    return key_bytes, iv_bytes

def encrypt_aes128_cbc(plaintext, key, iv):
    """
    Encrypt using AES-128-CBC matching CERSWebApi AESCryptography implementation
    """
    # Convert to UTF-8 (matches CERSWebApi Encoding.UTF8)
    plaintext_bytes = plaintext.encode('utf-8')
    
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

def generate_basic_auth_credentials():
    """
    Generate encrypted Basic Auth credentials for CERSWebApi GenerateToken endpoint
    """
    # Credentials from BasicAuthenticationAttribute.cs
    username = "CERS"
    password = "9JO9G3C7F05ZG1104"
    
    # Generate AES key and IV
    key, iv = generate_aes128_key_iv()
    
    # Encrypt credentials
    encrypted_username = encrypt_aes128_cbc(username, key, iv)
    encrypted_password = encrypt_aes128_cbc(password, key, iv)
    
    # URL encode the encrypted values
    url_encoded_username = urllib.parse.quote(encrypted_username)
    url_encoded_password = urllib.parse.quote(encrypted_password)
    
    # Create Basic Auth header
    credentials = f"{url_encoded_username}:{url_encoded_password}"
    basic_auth_header = base64.b64encode(credentials.encode('utf-8')).decode('utf-8')
    
    return {
        'username': username,
        'password': password,
        'encrypted_username': encrypted_username,
        'encrypted_password': encrypted_password,
        'url_encoded_username': url_encoded_username,
        'url_encoded_password': url_encoded_password,
        'basic_auth_header': basic_auth_header,
        'authorization_header': f"Basic {basic_auth_header}"
    }

if __name__ == "__main__":
    print("=== CERSWebApi Basic Auth Credentials Generator ===")
    print()
    
    # Generate credentials
    creds = generate_basic_auth_credentials()
    
    print("Original Credentials:")
    print(f"Username: {creds['username']}")
    print(f"Password: {creds['password']}")
    print()
    
    print("Encrypted Credentials:")
    print(f"Encrypted Username: {creds['encrypted_username']}")
    print(f"Encrypted Password: {creds['encrypted_password']}")
    print()
    
    print("URL Encoded Encrypted Credentials:")
    print(f"URL Encoded Username: {creds['url_encoded_username']}")
    print(f"URL Encoded Password: {creds['url_encoded_password']}")
    print()
    
    print("Basic Auth Header:")
    print(f"Authorization: {creds['authorization_header']}")
    print()
    
    print("=== Usage Example ===")
    print("For API calls to CERSWebApi/api/GenerateToken:")
    print("Headers:")
    print(f"  Authorization: {creds['authorization_header']}")
    print("  Content-Type: application/json")
    print()
    
    print("=== cURL Example ===")
    print(f'curl -X GET "http://10.146.2.8/CERSWebApi/api/GenerateToken" \\')
    print(f'  -H "Authorization: {creds['authorization_header']}" \\')
    print(f'  -H "Content-Type: application/json"')
    print()
    
    print("=== Key Information ===")
    print("- API expects AES-128 encrypted and URL-encoded credentials")
    print("- Uses first 16 chars of 'CERS&NicHP@23@ece' as key and IV")
    print("- Mode: CBC with PKCS7 padding")
    print("- Encoding: UTF-8")
    print("- Only required in RELEASE mode (not DEBUG)")
