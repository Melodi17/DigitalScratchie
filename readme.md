# Digital Scratchie
A framework client for reading `.scratchie` files.

## File format
The `.scratchie` file format consists of a header, mask, data, and two image files. The header is a 4-byte integer that represents the width of the image. The mask is a 4-byte integer that represents the height of the image. The data is a 2D array of integers that represents the image. The two image files are the original image and the scratched image, which are layered on top of each other, and then the mask is applied to the top image.

The file is structured as follows:

1. Header: A string in the format `st[ID];[set];[width];[height];` encoded in UTF-8.
2. Mask: A 2D array of bytes representing the scratch mask. Each byte is `1` if the corresponding position is scratched, otherwise `0`.
3. Base Layer: The original image saved as a byte array in PNG format.
4. Scratch Layer: The scratched image saved as a byte array in PNG format.
5. Data: Additional data encoded as a UTF-8 string.

Each section is preceded by a 4-byte integer indicating the length of the section. The sections are concatenated to form the final file.