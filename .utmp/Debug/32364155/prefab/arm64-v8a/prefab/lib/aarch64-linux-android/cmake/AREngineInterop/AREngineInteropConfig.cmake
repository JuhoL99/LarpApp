if(NOT TARGET AREngineInterop::AREngineInterop)
add_library(AREngineInterop::AREngineInterop SHARED IMPORTED)
set_target_properties(AREngineInterop::AREngineInterop PROPERTIES
    IMPORTED_LOCATION "C:/Users/lauren.juho-emil/.gradle/caches/8.11/transforms/874a11531efa8b23413ba59cd73ba822/transformed/jetified-AREngineInterop/prefab/modules/AREngineInterop/libs/android.arm64-v8a/libAREngineInterop.so"
    INTERFACE_INCLUDE_DIRECTORIES "C:/Users/lauren.juho-emil/.gradle/caches/8.11/transforms/874a11531efa8b23413ba59cd73ba822/transformed/jetified-AREngineInterop/prefab/modules/AREngineInterop/include"
    INTERFACE_LINK_LIBRARIES ""
)
endif()

